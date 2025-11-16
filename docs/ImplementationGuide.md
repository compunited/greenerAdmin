# Greener Admin – Implementation Guide

This document describes how to build the new Greener Admin Blazor application that will eventually replace or extend the existing WPF-based **GreenerConfigurator**. It summarizes the required functionality, architecture, and recommended implementation steps for the hosted Blazor solution.

## 1. Overall Goal

Deliver a modern, Office 365-style administrative experience that covers the full scope of the current WPF configurator. Administrators must be able to:

- Manage locations and their details
- Manage network, physical, and logical devices
- Manage SIM cards and bus connections
- Configure navigation categories, cards, and groups
- Configure rules (conditions, time windows, notification groups)
- Work with plan views (floorplans, device placements)
- Import sensors / unassigned devices
- Inspect device state and LoRaWAN data

The new app is a single-page application that uses the same backend API as the WPF client.

## 2. Technology and Architecture

### 2.1 Tech Stack

- **.NET 8** (or later)
- **ASP.NET Core hosted Blazor WebAssembly**
  - `GreenerConfigurator.Web.Server` – host, API gateway, auth, static assets
  - `GreenerConfigurator.Web.Client` – WASM SPA
- **Shared client library** – `GreenerConfigurator.ClientCore`
  - Shared models, services, and API helpers reused by WPF and Blazor
- **Authentication** – Azure Entra ID (Azure AD) with OpenID Connect/OAuth2 and MSAL
- **UI** – Blazor + Fluent/Office-like component library, Microsoft 365 admin style layout

### 2.2 Target Solution Structure

Add three new projects to `GreenerConfigurator.sln`:

1. `GreenerConfigurator.ClientCore` – shared class library
2. `GreenerConfigurator.Web.Server` – ASP.NET Core host
3. `GreenerConfigurator.Web.Client` – Blazor WebAssembly client

### 2.3 Refactoring Steps

#### Step 1 – Introduce ClientCore

1. Create the `GreenerConfigurator.ClientCore` project (`net8.0`).
2. Move the WPF client `Models`, `Services`, and `Utilities/Api/ApiHelper.cs` into ClientCore.
3. Reference the required `Greener.Web.Definitions.*` assemblies from ClientCore.
4. Update the WPF project to reference ClientCore and ensure it still compiles.

#### Step 2 – Set Up Hosted Blazor WebAssembly

1. Add `GreenerConfigurator.Web.Server` via the hosted Blazor template.
2. Add `GreenerConfigurator.Web.Client` and reference ClientCore.
3. Register or wrap ClientCore services for dependency injection.

#### Step 3 – Authentication and Authorization

1. Configure Azure Entra ID auth in the server project.
2. Secure the SPA route and configure backend API access tokens.
3. Integrate MSAL in the WASM client and attach bearer tokens to API calls.
4. Optionally implement role-based authorization using claims.

#### Step 4 – UI Mapping from WPF to Web

For each WPF View/ViewModel pair, build the corresponding Razor component/page using the shared services. Examples:

| WPF View | Blazor Page |
| --- | --- |
| `Views/Location/LocationManagementView.xaml` | `Pages/Locations/LocationManagement.razor` |
| `Views/Network/NetworkDeviceManagementView.xaml` | `Pages/Devices/NetworkDeviceManagement.razor` |
| `Views/Rule/RuleManagementView.xaml` | `Pages/Rules/RuleManagement.razor` |

Focus on the UX patterns used in the WPF app (lists, filters, detail dialogs) and recreate them with the Fluent UI components.

## 3. Backend API Reference

The Blazor application should call the same `/api/1.0/...` endpoints as the WPF client through the shared services. The table below lists the endpoints discovered in the existing WPF services and the models they deserialize into. Treat calls without deserialization as commands.

<details>
<summary>Endpoint list</summary>

- `/api/1.0/ActiveTime/Add` → `ActiveTimeEditModel`
- `/api/1.0/ActiveTime/Delete` → `ActiveTimeEditModel`
- `/api/1.0/ActiveTime/Edit` → `ActiveTimeEditModel`
- `/api/1.0/CompareCondition/Add` → `CompareConditionEditModel`
- `/api/1.0/CompareCondition/Delete` → `CompareConditionEditModel`, `NotificationGroupDataEditModel`
- `/api/1.0/CompareCondition/Edit` → `CompareConditionEditModel`, `NotificationGroupDataEditModel`
- `/api/1.0/Location/LocationDetail/Add` → `LocationDetailModel`
- `/api/1.0/Location/LocationDetail/ByLocationId` → `List<LocationDetailModel>`
- `/api/1.0/Location/LocationDetail/Edit` → `LocationDetailModel`
- `/api/1.0/LocationDetail/Add` → `LocationDetailModel`
- `/api/1.0/NetworkDevice/NetworkDeviceByNetworkDeviceId` → `NetworkDeviceEditModel`
- `/api/1.0/NetworkDevice/NetworkDevices` → `List<NetworkDeviceViewModel>`
- `/api/1.0/NetworkDevice/NetworkDevicesForLocationDetailId` → `List<NetworkDeviceViewModel>`
- `/api/1.0/NetworkDevice/NetworkDevicesForLocationId` → `List<NetworkDeviceViewModel>`
- `/api/1.0/NetworkDevice/UnassignedNetworkDevice/Add` → `List<AddUnassignedNetworkDeviceRequestResultDto>`
- `/api/1.0/NetworkDevice/UnassignedNetworkDevice/GetUnassigned` → `List<UnassignedNetworkDeviceDto>`
- `/api/1.0/NotificationGroup/GetAll` → `List<NotificationGroupViewDto>`
- `/api/1.0/NotificationGroupData/Add` → `NotificationGroupDataEditModel`
- `/api/1.0/Rule/Add` → `RuleEditModel`
- `/api/1.0/Rule/Edit` → `RuleEditModel`
- `/api/1.0/Rule/GetAll` → `List<RuleViewDto>`
- `/api/1.0/Rule/GetRuleByRuleId` → `RuleEditModel`
- `/api/1.0/RuleDetail/Add` → `RuleDetailEditModel`
- `/api/1.0/RuleDetail/Delete` → command/void
- `/api/1.0/RuleDetail/Edit` → `RuleDetailEditModel`
- `/api/1.0/RuleDetail/GetRuleByRuleId` → `RuleDetailEditModel`
- `/api/1.0/SimCard/GetSimCardsByNetworkDeviceId` → `List<SimCardViewModel>`
- `/api/1.0/SimCard/GetSimCardsByNetworkDeviceIdAndSimCardId` → `SimCardEditModel`
- `/api/1.0/location` → `List<LocationModel>`
- `/api/1.0/BusConnection/Add` → `T`
- `/api/1.0/BusConnection/Edit` → `T`
- `/api/1.0/DeviceState/ByLocationId` → `List<DeviceStateViewDto>`
- `/api/1.0/Location/Add` → `LocationModel`
- `/api/1.0/Location/Edit` → `LocationModel`
- `/api/1.0/LogicalDevice` → `LogicalDeviceModel`
- `/api/1.0/LogicalDevice/ByDeviceCategoryId` → `List<LogicalDeviceModel>`
- `/api/1.0/LogicalDevice/ByLocationId` → `List<LogicalDeviceModel>`
- `/api/1.0/LogicalDeviceNavigationCard/Add` → `NavigationCardModel`
- `/api/1.0/LogicalDeviceNavigationCard/ByLocationId` → `List<NavigationCardModel>`
- `/api/1.0/LogicalDeviceNavigationCard/ByNavigationCardGroupId/CardList` → `List<NavigationCardModel>`
- `/api/1.0/LogicalDeviceNavigationCard/ByNavigationCategoryId/CardList` → `List<NavigationCardModel>`
- `/api/1.0/LogicalDeviceNavigationCard/Delete` → command/void
- `/api/1.0/LogicalDeviceNavigationCard/DeleteFromGroup` → `NavigationCardModel`
- `/api/1.0/LogicalDeviceNavigationCard/Edit` → `NavigationCardModel`
- `/api/1.0/LogicalDeviceNavigationCard/Group/Add` → `NavigationCardGroupModel`
- `/api/1.0/LogicalDeviceNavigationCard/Group/Delete` → command/void
- `/api/1.0/LogicalDeviceNavigationCard/Group/Edit` → `NavigationCardGroupModel`
- `/api/1.0/LogicalDeviceNavigationCard/Group/M2M/Add` → command/void
- `/api/1.0/LogicalDeviceNavigationCard/Group/M2M/Delete` → command/void
- `/api/1.0/LogicalDeviceNavigationCard/M2M/Add` → `object`
- `/api/1.0/LogicalDeviceNavigationCard/M2M/Delete` → `object`
- `/api/1.0/LogicalDeviceNavigationCard/M2M/UpdateSortNumberAndDataDetailNumber` → command/void
- `/api/1.0/LogicalDeviceNavigationCard/SetGroup` → `NavigationCardModel`
- `/api/1.0/LoraWanDataRows/ByLocationDetailId` → `List<LoraWanDataRowsModel>`
- `/api/1.0/LoraWanDataRows/ByLocationId` → `List<LoraWanDataRowsModel>`
- `/api/1.0/NetworkDevice/Add` → `NetworkDeviceEditModel`
- `/api/1.0/NetworkDevice/BusConnection/Delete` → command/void
- `/api/1.0/NetworkDevice/Delete` → command/void
- `/api/1.0/NetworkDevice/Edit` → `NetworkDeviceEditModel`
- `/api/1.0/PhysicalDevice/ByLogicalDeviceId/CurrentDevice` → `PhysicalDeviceModel`
- `/api/1.0/PlanViewForGreenerConfigurator/CreateSavePlanView` → command/void
- `/api/1.0/PlanViewForGreenerConfigurator/UpdatePlanView` → command/void
- `/api/1.0/PlanViewFrontend/GetPlanViewByPlanViewRefId` → `PlanViewWithDataDto`
- `/api/1.0/PlanViewFrontend/GetPlanViewsByLocationId` → `List<PlanItem>`
- `/api/1.0/Rule/Delete` → command/void
- `/api/1.0/SimCard/Add` → `SimCardEditModel`
- `/api/1.0/SimCard/Delete` → command/void
- `/api/1.0/SimCard/Edit` → `SimCardEditModel`
- `/api/1.0/UnassignedPhysicalDevice/GetManufacturerAndDeviceInfo` → `ManufacturerAndDeviceInfoDto`
- `/api/1.0/UnassignedPhysicalDevice/Importkeys` → `List<UnassignedPhysicalDeviceDto>`

</details>

## 4. Suggested Implementation Order

1. **Locations & Location Details** – CRUD for locations and nested details.
2. **Network Devices & Bus Connections** – views by location/detail, unassigned devices, bus connections.
3. **Logical & Physical Devices** – lists filtered by location/category, mapping to physical devices.
4. **SIM Cards** – CRUD and integrations with devices.
5. **Navigation** – admin UI for categories, cards, groups.
6. **Rules & Notifications** – rule list, create/edit workflows, rule details.
7. **Plan Views** – plan view lists per location plus create/update flows.
8. **Device State, LoRaWAN & Imports** – data views and import flows.

## 5. Expected Deliverables

- Hosted Blazor WASM solution with authentication and backend integration
- Shared client logic extracted to `GreenerConfigurator.ClientCore`
- UI mapping between WPF and Blazor components
- Documentation (XML summaries, code comments) for key services and pages

> ✅ This repository already ships with a minimal hosted solution located in `src/`. The implementation includes the
> shared `ClientCore` library together with a starter Locations and Rules experience so that future contributors can focus
> on fleshing out the remaining feature areas.

