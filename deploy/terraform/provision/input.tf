variable "arm_subscription_id" {}
variable "arm_client_id" {}
variable "arm_client_secret" {}
variable "arm_tenant_id" {}

variable "environment_name" {}
variable "location" { default = "eastus2" }

locals {
  project_name = "demotf"

  tags = {
    project     = local.project_name,
    environment = "default_should_be_set"
  }
}
