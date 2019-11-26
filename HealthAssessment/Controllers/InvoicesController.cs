using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using HealthAssessment.Models;
using HealthAssessment.Services;

namespace HealthAssessment.Controllers
{
  public class InvoicesController : Controller
  {
    private HospitalEntities db = new HospitalEntities();

   // GET: Invoices
    public async Task<ActionResult> Index()
    {
      var invoices = db.Invoices.Include(i => i.Patient);
      return View(await invoices.ToListAsync());
    }

    // GET: Invoices/Details/5
    public async Task<ActionResult> Details(long? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Invoice invoice = await db.Invoices.FindAsync(id);
      if (invoice == null)
      {
        return HttpNotFound();
      }
      return View(invoice);
    }

    // GET: Invoices/Create
    public ActionResult Create()
    {
    
      ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FirstName");
      return View();
    }

    // POST: Invoices/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create([Bind(Include = "InvoiceId,InvoiceDateTime,PatientId,InvoiceTotal")] Invoice invoice)
    {
      if (ModelState.IsValid)
      {
        db.Invoices.Add(invoice);
        await db.SaveChangesAsync();
        return RedirectToAction("Index");
      }

      ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FirstName", invoice.PatientId);
      return View(invoice);
    }

    // GET: Invoices/Edit/5
    public async Task<ActionResult> Edit(long? id)
    {
      string apiUrl = $"https://localhost:44333/api/invoice/{id}";

      using (HttpClient client = new HttpClient())
      {
        client.BaseAddress = new Uri(apiUrl);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        HttpResponseMessage response = client.GetAsync(apiUrl).Result;
        if (response.IsSuccessStatusCode)
        {
          var data = response.Content.ReadAsStringAsync().Result;
          var invoice = Newtonsoft.Json.JsonConvert.DeserializeObject<Invoice>(data);

          var viewModel = new Invoice
          {
            InvoiceDateTime = invoice.InvoiceDateTime,
            InvoiceTotal = invoice.InvoiceTotal,
            PatientId = invoice.PatientId,
            Patient = invoice.Patient,
            InvoiceId = invoice.InvoiceId,
            InvoiceLines = invoice.InvoiceLines
          };
          return View("_Layout", viewModel);
        }

      }
      return View("Index");

    }

    // POST: Invoices/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit([Bind(Include = "InvoiceId,InvoiceDateTime,PatientId,InvoiceTotal")] Invoice invoice)
    {
      if (ModelState.IsValid)
      {
        db.Entry(invoice).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return RedirectToAction("Index");
      }
      ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FirstName", invoice.PatientId);
      return View(invoice);
    }

    // GET: Invoices/Delete/5
    public async Task<ActionResult> Delete(long? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Invoice invoice = await db.Invoices.FindAsync(id);
      if (invoice == null)
      {
        return HttpNotFound();
      }
      return View(invoice);
    }

    // POST: Invoices/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(long id)
    {
      Invoice invoice = await db.Invoices.FindAsync(id);
      db.Invoices.Remove(invoice);
      await db.SaveChangesAsync();
      return RedirectToAction("Index");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        db.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
