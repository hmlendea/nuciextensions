# Privacy and Personal Data

NuciExtensions is a .NET NuGet package providing extension methods for common types. It contains no runtime data collection, telemetry, or external integrations.

**Information reviewed:** 2026-10-09

## 📑 Table of Contents

- [What This Document Covers](#what-this-document-covers)
- [Self-Hosted Deployments](#self-hosted-deployments)
- [Data We Handle](#data-we-handle)
- [Processing and Use](#processing-and-use)
- [Storage, Retention, and Deletion](#storage-retention-and-deletion)
- [External Processing and Integrations](#external-processing-and-integrations)
- [Document Changes](#document-changes)
- [Contact](#contact)

## 🔎 What This Document Covers

This document describes how NuciExtensions at https://github.com/hmlendea/nuciextensions handles personal data. It covers the library behaviour and verified integrations described below. Where the software is self-hosted, the instance operator may have separate responsibilities described below.

## 🏠 Self-Hosted Deployments

NuciExtensions is a .NET library distributed as a NuGet package. It is not a service or application that can be deployed independently. Consumers reference the package in their own applications. The library itself has no deployment model, configuration, local storage, logs, backups, access controls, retention, or request handling. Consumers control all data handling within their applications.

## 📥 Data We Handle

### Data Provided to the Application

No personal data is requested or accepted by the library. Extension methods operate solely on data passed as method arguments by the caller.

### Data Generated or Collected by the Application

No personal data is generated or collected automatically by the library. The library performs no logging, telemetry, crash reporting, or update checks.

### Data Received from Integrations

No personal data is received from integrations or third parties. The library has no built-in integrations.

## 🧭 Processing and Use

The library processes no personal data. Extension methods perform deterministic transformations on caller-provided in-memory values and return results directly to the caller.

## 🗄️ Storage, Retention, and Deletion

The library stores no data. All operations are in-memory and stateless. No databases, files, logs, caches, or backups are used by the library.

## 🔗 External Processing and Integrations

The library has no built-in external data transfer, integrations, or third-party services.

| Service or integration | Purpose | Data involved | Configuration or documentation |
|-----------------------|---------|---------------|--------------------------------|
| None | N/A | N/A | N/A |

## 🔄 Document Changes

Update this document when library data flows, storage, integrations, or deployment responsibilities change. The current version is published at https://github.com/hmlendea/nuciextensions/blob/master/PRIVACY.md.

## 📬 Contact

For questions about library data handling, contact the project maintainers via GitHub issues at https://github.com/hmlendea/nuciextensions/issues. Do not send passwords, access tokens, or other secrets.