# Legacy Orders API

> **Deliberately insecure and outdated: test fixture for health scanning. Never deploy.**

This is the TechDocs site for [`bad-app-sample`](https://github.com/demoncartel/bad-app-sample), a small ASP.NET Core Web API used as a known-bad target for repository health scanners (framework currency, vulnerable packages, tests, and basics).

It is a "legacy orders" service targeting end-of-life `net6.0`. It is not a real product and must not be deployed.

## API

The service exposes three endpoints:

- `GET /api/orders` — list orders
- `GET /api/orders/{id}` — get one order
- `POST /api/orders` — create an order

The OpenAPI definition used by the Backstage API entity is [openapi.yaml](./openapi.yaml).

## Known issues

Every planted defect is listed in the repository-root [KNOWN_ISSUES.md](https://github.com/demoncartel/bad-app-sample/blob/main/KNOWN_ISSUES.md) answer key. Do not "fix" those issues; they exist so a health scanner has something to detect.

A short [README](https://github.com/demoncartel/bad-app-sample/blob/main/README.md) is also at the repo root, mainly so visitors see the never-deploy warning.

## Backstage

In the catalog this repository is the `legacy-orders-api` component (type `service`, lifecycle `deprecated`) in the `repo-health-samples` system, owned by `group:demoncartel/platform`.
