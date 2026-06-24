using RealEstate.Domain.Common;
using RealEstate.Domain.Common.Enums;

namespace RealEstate.Domain.Entities
{
    public class Appointment : BaseEntity
    {
        // ── Schedule ──────────────────────────────────────
        public DateTime ScheduledAt { get; set; }
        public DateTime? ActualVisitAt { get; set; }   // when they actually visited
        public int? DurationMinutes { get; set; }       // expected visit duration

        // ── Status ────────────────────────────────────────
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
        // Pending, Confirmed, Done, Cancelled, NoShow

        // ── Feedback ──────────────────────────────────────
        public string? Notes { get; set; }
        public string? Feedback { get; set; }           // client feedback after visit
        public AppointmentResult? Result { get; set; }
        // Interested, NotInterested, NeedsFollowUp

        public string? CancellationReason { get; set; }
        public DateTime? CancelledAt { get; set; }

        // ── Relations ─────────────────────────────────────
        public Guid PropertyId { get; set; }            // FK only
        public Guid ClientId { get; set; }              // FK only
        public Guid AgentId { get; set; }               // FK only

        public Guid CompanyId { get; set; }
        public Company Company { get; set; } = null!;
    }
}