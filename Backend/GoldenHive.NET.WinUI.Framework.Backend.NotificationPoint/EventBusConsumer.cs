// =============================================================================
// Project : GoldenHive.NET.WinUI.Framework.Backend.NotificationPoint
// File    : EventBusConsumer.cs
// Layer   : Backend / Docker
// Status  : Architecture sketch - comments only, no implementation yet
// =============================================================================
//
// PURPOSE
//   Reads events from the Docker modules off the event bus (e.g. RabbitMQ or Kafka).
//
// BEHAVIOUR
//   - Convert the event into a NotificationEnvelope, determine the audience, persist it.
//   - Process idempotently (event ID).
