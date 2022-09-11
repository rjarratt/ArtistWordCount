Feature: MusicMetadataPortContract
	As a developer
	I want to use the music metadata port
	So I can query for music metadata
	I also want to ensure that the emulated and Http adapters both fulfil the same port contract

@Contract
Scenario Outline: a query that identifies a single artist returns that artist
	Given I am using the <Type> music metadata adapter
	When I query for 'Kate Bush'
	Then I get the following list of artists:
		| Name       | MbId                                 |
		| Bush, Kate | 4b585938-f271-45e2-b19a-91c634b5e396 |

Examples:
	| Type     |
#	| Emulated |
	| Http     |

@Integration
Scenario: a query that is repeated faster than the rate limit succeeds
	Given I am using the Http music metadata adapter
	When I query for 'Kate Bush' 20 times in a row
	#Then no error occurs
