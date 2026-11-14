// =============================================================================
// Project : GoldenHive.NET.Modules.DocumentManagement.Backend
// File    : ApproveInvoice.cs
// Layer   : Module DocumentManagement
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Example use case in the monad architecture: approve an invoice. (Described, not implemented.)
//
// PIPELINE
//   1. Validate   : invoiceId present, comment max 500 chars              -> Validation
//   2. Authorize  : user (or RunAs user of a workflow / AI run) has documents.approve -> Forbidden
//   3. Load       : invoice by ID                                          -> Option -> NotFound
//   4. Decide     : pure domain rule on the invoice record:
//                     - status must be "Submitted"           else Conflict "documents.invoice.notSubmitted"
//                     - amount <= approver's limit           else Forbidden "documents.invoice.overLimit"
//                     - approver is not the submitter        else Forbidden "documents.invoice.selfApproval"
//                   returns the approved invoice + InvoiceApproved event
//   5. Persist    : save invoice (Try: concurrency conflict -> Conflict)   inside TransactionStep
//   6. Publish    : InvoiceApproved into the outbox (same transaction)     -> NotificationPoint, workflows
//   7. Project    : map to ApproveInvoiceResponse
//   Result -> HTTP 200 / 400 / 403 / 404 / 409 via ResultHttpMapper.
//
// SAME CODE FOR
//   The HTTP endpoint, the workflow step type "documents.approveInvoice" and the AI tool of the same
//   name - identical rules and errors for humans, the workflow engine and the AI agent.
