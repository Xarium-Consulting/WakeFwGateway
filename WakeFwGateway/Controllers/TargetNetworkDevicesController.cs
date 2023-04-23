using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WakeFwGateway.Data;
using WakeFwGateway.Models;
using System.Net;
using System.Net.NetworkInformation;
using static WakeFwGateway.WakeTarget;
using WakeFwGateway.Data.Enumeration;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WakeFwGateway.Controllers
{
    public class TargetNetworkDevicesController : Controller
    {
        private readonly WakeFwGatewayContext _context;
        private List<Notification> _messages = new();

        public TargetNetworkDevicesController(WakeFwGatewayContext context)
        {
            _context = context;
        }

        // GET: TargetNetworkDevices
        public async Task<IActionResult> Index()
        {
            return _context.TargetNetworkDevice != null ?
                        View(await _context.TargetNetworkDevice.ToListAsync()) :
                        Problem("Entity set 'WakeFwGatewayContext.TargetNetworkDevice'  is null.");
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
                if (!TargetNetworkDeviceExists(targetNetworkDevice.Id))
                {
                    AddNotification($"No device found for specified id", NotificationType.failure);
                    return View("Index");
                }
                try
                {
                    _context.Update(targetNetworkDevice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    AddNotification("Failed to edit device" , NotificationType.failure);
                    return View("Index");
                }
                catch (Exception)
                {
                    AddNotification("Failed to edit device, unknown exception occured", NotificationType.failure);
                    return View("Index");
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
                return Problem("Entity set 'WakeFwGatewayContext.TargetNetworkDevice'  is null.");
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
                return Problem("Entity set 'WakeFwGatewayContext.TargetNetworkDevice'  is null.");
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
            _messages.Add(notification);
            ViewData["Messages"] = _messages;
        }
    }
}
