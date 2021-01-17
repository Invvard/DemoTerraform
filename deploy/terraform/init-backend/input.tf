variable "subscription_id" {}
variable "client_id" {}
variable "client_secret" {}
variable "tenant_id" {}

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
