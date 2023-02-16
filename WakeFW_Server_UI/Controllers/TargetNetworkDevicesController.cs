﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WakeFW_Server_UI.Data;
using WakeFW_Server_UI.Models;
using System.Net;
using System.Net.NetworkInformation;
using static WakeFW_Server_UI.WakeTarget;
using WakeFW_Server_UI.Data.Enumeration;

namespace WakeFW_Server_UI.Controllers
{
    public class TargetNetworkDevicesController : Controller
    {
        private readonly WakeFW_Server_UIContext _context;

        public TargetNetworkDevicesController(WakeFW_Server_UIContext context)
        {
            _context = context;
        }
        internal List<Notification> Messages
        {
            get
            {
                if (Messages != null) { return Messages; }
                return new List<Notification>();
            }
        }

        // GET: TargetNetworkDevices
        public async Task<IActionResult> Index()
        {
            return _context.TargetNetworkDevice != null ?
                        View(await _context.TargetNetworkDevice.ToListAsync()) :
                        Problem("Entity set 'WakeFW_Server_UIContext.TargetNetworkDevice'  is null.");
        }

        // GET: TargetNetworkDevices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TargetNetworkDevice == null)
            {
                AddNotification($"Error loading device data", NotificationType.failure);
                return View("Index");
            }

            var targetNetworkDevice = await _context.TargetNetworkDevice
                .FirstOrDefaultAsync(m => m.Id == id);
            if (targetNetworkDevice == null)
            {
                AddNotification($"No device found for specified id", NotificationType.failure);
                return View("Index");
            }

            return View(targetNetworkDevice);
        }

        // GET: TargetNetworkDevices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TargetNetworkDevices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ip,Mac,AddedDate")] TargetNetworkDevice targetNetworkDevice)
        {
            targetNetworkDevice.AddedDate = DateTime.Now;

            ModelState.ClearValidationState(nameof(TargetNetworkDevice));
            if (TryValidateModel(targetNetworkDevice, nameof(TargetNetworkDevice)))
            {
                _context.Add(targetNetworkDevice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            AddNotification($"Device {targetNetworkDevice.Mac} has been added.", NotificationType.success);
            return View(targetNetworkDevice);
        }

        // GET: TargetNetworkDevices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TargetNetworkDevice == null)
            {
                AddNotification($"Error loading device data", NotificationType.failure);
                return View("Index");
            }

            var targetNetworkDevice = await _context.TargetNetworkDevice.FindAsync(id);
            if (targetNetworkDevice == null)
            {
                AddNotification($"No device found for specified id", NotificationType.failure);
                return View("Index");
            }

            return View(targetNetworkDevice);
        }

        // POST: TargetNetworkDevices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ip,Mac,AddedDate")] TargetNetworkDevice targetNetworkDevice)
        {
            if (id != targetNetworkDevice.Id)
            {
                AddNotification($"Error loading device data", NotificationType.failure);
                return View("Index");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(targetNetworkDevice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TargetNetworkDeviceExists(targetNetworkDevice.Id))
                    {
                        AddNotification($"No device found for specified id", NotificationType.failure);
                        return View("Index");
                    }
                    else
                    {
                        throw;
                    }
                }
                AddNotification($"Device {targetNetworkDevice.Mac} has been modified.", NotificationType.success);
                return View("Index", await _context.TargetNetworkDevice.ToListAsync());
            }
            return View(targetNetworkDevice);
        }

        // GET: TargetNetworkDevices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TargetNetworkDevice == null)
            {
                AddNotification($"Error loading device data", NotificationType.failure);
                return View("Index");
            }

            var targetNetworkDevice = await _context.TargetNetworkDevice
                .FirstOrDefaultAsync(m => m.Id == id);
            if (targetNetworkDevice == null)
            {
                AddNotification($"No device found for specified id", NotificationType.failure);
                return View("Index");
            }

            return View(targetNetworkDevice);
        }

        // POST: TargetNetworkDevices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TargetNetworkDevice == null)
            {
                return Problem("Entity set 'WakeFW_Server_UIContext.TargetNetworkDevice'  is null.");
            }
            var targetNetworkDevice = await _context.TargetNetworkDevice.FindAsync(id);
            if (targetNetworkDevice != null)
            {
                _context.TargetNetworkDevice.Remove(targetNetworkDevice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ValidateIpAddress(string ip)
        {
            IPAddress parsedIp;
            if (!IPAddress.TryParse(ip, out parsedIp))
            {
                return Json("Invalid IP address");
            }
            return Json(true);
        }

        public IActionResult ValidateMacAddress(string mac)
        {
            PhysicalAddress parsedMac;
            if (!PhysicalAddress.TryParse(mac, out parsedMac))
            {
                return Json("Invalid MAC address");
            }
            return Json(true);
        }
        public async Task<IActionResult> Wake(int id, uint repetitions = 10)
        {
            if (_context.TargetNetworkDevice == null)
            {
                return Problem("Entity set 'WakeFW_Server_UIContext.TargetNetworkDevice'  is null.");
            }
            var target = await _context.TargetNetworkDevice.FindAsync(id);
            if (target == null)
            {
                AddNotification("Invalid target id", NotificationType.failure);
                return View("Index", await _context.TargetNetworkDevice.ToListAsync());
            }
            IPAddress targetAddress = WakeTarget.GetBroadcastAddress(IPAddress.Parse(target.Ip), IPAddress.Parse("255.255.255.255"));
            byte[] payload = WakeTarget.GeneratePayload(PhysicalAddress.Parse(target.Mac));
            WakeDevice(targetAddress, payload, null, repetitions);

            AddNotification($"WOL packets have been sent to: {target.Mac}", NotificationType.neutral);
            return View("Index", await _context.TargetNetworkDevice.ToListAsync());
        }

        private bool TargetNetworkDeviceExists(int id)
        {
            return (_context.TargetNetworkDevice?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private void AddNotification(string message, NotificationType type)
        {
            Notification notification = new()
            {
                Message = message,
                Type = type
            };
            Messages.Add(notification);
            ViewData["Messages"] = Messages;
        }
    }
}
