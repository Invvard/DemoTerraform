variable "subscription_id" {}
variable "client_id" {}
variable "client_secret" {}
variable "tenant_id" {}

variable "environment_name" {}
variable "location" { default = "eastus2" }

locals {
  project_name = "demotf"

  tags = {
    project     = local.project_name,
    environment = "default_should_be_set"
  }
}
