using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HealthAssessment.Models;

namespace HealthAssessment.Controllers
{
  public class InvoiceLinesController : Controller
  {
    private HospitalEntities db = new HospitalEntities();

    // GET: InvoiceLines
    public async Task<ActionResult> Index()
    {
      var invoiceLines = db.InvoiceLines.Include(i => i.Invoice);
      return View(await invoiceLines.ToListAsync());
    }

    // GET: InvoiceLines/Details/5
    public async Task<ActionResult> Details(long? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      InvoiceLine invoiceLine = await db.InvoiceLines.FindAsync(id);
      if (invoiceLine == null)
      {
        return HttpNotFound();
      }
      return View(invoiceLine);
    }

    // GET: InvoiceLines/Create
    public ActionResult CreateInvoiceLine()
    {

      //ViewBag.InvoiceId = new SelectList(db.Invoices, "InvoiceId", "InvoiceId");
      //return View();
    }

    // POST: InvoiceLines/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create([Bind(Include = "InvoiceLineId,InvoiceId,Qty,Code,Description,LineTotal")] InvoiceLine invoiceLine)
    {
      if (ModelState.IsValid)
      {
        db.InvoiceLines.Add(invoiceLine);
        await db.SaveChangesAsync();
        return RedirectToAction("Index");
      }

      ViewBag.InvoiceId = new SelectList(db.Invoices, "InvoiceId", "InvoiceId", invoiceLine.InvoiceId);
      return View(invoiceLine);
    }

    // GET: InvoiceLines/Edit/5
    public async Task<ActionResult> Edit(long? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      InvoiceLine invoiceLine = await db.InvoiceLines.FindAsync(id);
      if (invoiceLine == null)
      {
        return HttpNotFound();
      }
      ViewBag.InvoiceId = new SelectList(db.Invoices, "InvoiceId", "InvoiceId", invoiceLine.InvoiceId);
      return View(invoiceLine);
    }

    // POST: InvoiceLines/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit([Bind(Include = "InvoiceLineId,InvoiceId,Qty,Code,Description,LineTotal")] InvoiceLine invoiceLine)
    {
      if (ModelState.IsValid)
      {
        db.Entry(invoiceLine).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return RedirectToAction("Index");
      }
      ViewBag.InvoiceId = new SelectList(db.Invoices, "InvoiceId", "InvoiceId", invoiceLine.InvoiceId);
      return View(invoiceLine);
    }

    // GET: InvoiceLines/Delete/5
    public async Task<ActionResult> Delete(long? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      InvoiceLine invoiceLine = await db.InvoiceLines.FindAsync(id);
      if (invoiceLine == null)
      {
        return HttpNotFound();
      }
      return View(invoiceLine);
    }

    // POST: InvoiceLines/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(long id)
    {
      InvoiceLine invoiceLine = await db.InvoiceLines.FindAsync(id);
      db.InvoiceLines.Remove(invoiceLine);
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
