variable "subscription_id" {}
variable "client_id" {}
variable "client_secret" {}
variable "tenant_id" {}

variable "environment_name" {}
variable "location" { default = "eastus2" }

locals {
  project_name = "demotf"
  resource_prefix = "${local.project_name}-${var.environment_name}"

  tags = {
    project     = local.project_name,
    environment = "default_should_be_set"
  }
}
