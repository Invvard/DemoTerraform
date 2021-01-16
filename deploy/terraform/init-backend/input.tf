variable "arm_subscription_id" {}
variable "arm_client_id" {}
variable "arm_client_secret" {}
variable "arm_tenant_id" {}

variable "resource_group_name" {}
variable "storage_account_name" {}
variable "container_tfstate_name" {}

variable "resources_location" {
  default = "eastus2"
}

variable "storage_tier" {
  default = "Standard"
}

variable "storage_replication_type" {
  default = "LRS"
}
