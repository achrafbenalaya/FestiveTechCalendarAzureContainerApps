resource "azurerm_resource_group" "rg" {
  name     = "rg-aca-terraform-01"
  location = "francecentral"
}

resource "azurerm_log_analytics_workspace" "workspace" {
  name                = "workspace-aca"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  sku                 = "PerGB2018"
  retention_in_days   = 30
}

# resource "azurerm_container_registry" "acr" {
#   name                     = "acrhxh"
#   resource_group_name      = azurerm_resource_group.rg.name
#   location                 = azurerm_resource_group.rg.locationn
#   sku                      = "Standard"
#   admin_enabled            = false
# }

# resource "azurerm_managed_identity" "identity" {
#   name                = "managedhxhidenitty"
#   resource_group_name = azurerm_resource_group.rg.name
# }


# resource "azurerm_role_assignment" "acr_pull" {
#   scope                = azurerm_container_registry.acr.id
#   role_definition_name = "AcrPull"
#   principal_id         = azurerm_managed_identity.identity.principal_id
# }

resource "azurerm_container_app_environment" "aca_environment" {
  name                       = "aca-environment-01"
  location                   = azurerm_resource_group.rg.location
  resource_group_name        = azurerm_resource_group.rg.name
  log_analytics_workspace_id = azurerm_log_analytics_workspace.workspace.id
}

resource "azurerm_container_app" "aca_backend_internal" {
  name                         = "aca-hxh-backend-api-001"
  container_app_environment_id = azurerm_container_app_environment.aca_environment.id
  resource_group_name          = azurerm_resource_group.rg.name
  revision_mode                = "Single"
    registry {
    server               = "myregistry.azurecr.io"
    username             = ""
    password_secret_name = "clientsecretname"
  }
  template {
    container {
      name   = "backend-api"
      image  = "acrfestivetechdev.azurecr.io/hunterxhunterapi:v1"
      cpu    = 0.25
      memory = "0.5Gi"
    }
  }

  ingress {
    allow_insecure_connections = false
    external_enabled           = false
    target_port                = 3500
    transport                  = "auto"

    traffic_weight {
      latest_revision = true
      percentage      = 100
    }
  }
}

resource "azurerm_container_app" "aca_frontend" {
  name                         = "aca-hxh-frontend-001"
  container_app_environment_id = azurerm_container_app_environment.aca_environment.id
  resource_group_name          = azurerm_resource_group.rg.name
  revision_mode                = "Single"
    registry {
    server               = "myregistry.azurecr.io"
    username             = ""
    password_secret_name = "clientsecretname"
  }
  template {
    container {
      name   = "frontend-ui"
      image  = "acrfestivetechdev.azurecr.io/hunterxhunterfrt:v1"
      cpu    = 0.25
      memory = "0.5Gi"

      env {
        name  = "API_BASE_URL"
        value = "https://${azurerm_container_app.aca_backend_internal.latest_revision_fqdn}"
      }
    }
  }

  ingress {
    allow_insecure_connections = false
    external_enabled           = true
    target_port                = 80
    transport                  = "auto"

    traffic_weight {
      latest_revision = true
      percentage      = 100
    }
  }
}

output "app_frontend_url" {
  value = azurerm_container_app.aca_frontend.latest_revision_fqdn
}