Feature: PostName
	In order to greet visitors
	As a user
	I want to receive a custom message

@postName
Scenario Outline: Receive a custom message
	Given I have assigned '<paramValue>' to the body parameter '<paramName>'
	When I request the PostName API
	Then the response should be '<expectedResponse>'

	Examples: 
		| paramValue | paramName | expectedResponse                                                                                         |
		| Luke       | name      | Hello, Luke. This HTTP triggered function executed successfully.                                         |
		| Leia       | name      | Hello, Leia. This HTTP triggered function executed successfully.                                         |

@postName
Scenario Outline: Receive an error message
	Given I send an incorrect '<paramName>' in the body
	When I request the PostName API
	Then the response should be 'This HTTP triggered function executed successfully. Pass a name in the body for a personalized response.'
	
	Examples: 
		| paramName |
		| another   |
		|           |