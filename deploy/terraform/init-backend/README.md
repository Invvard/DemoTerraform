# Description
Build the base infrastructure that will receive data to support future deployment, in particular the Terraform backend.

# Tools version
* Terraform : >= 0.14
* AzureRM : >= 2.43

# Variables
Those variables must be set within the deployment pipeline :
1. arm_subscription_id
2. arm_client_id
3. arm_client_secret
4. arm_tenant_id

Those should be concealed as soon as they are set into ADO pipeline, using a Library secure file for example.
