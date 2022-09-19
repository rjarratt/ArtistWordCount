Feature: MusicMetadataPortContract-ArtistQuery
	As a developer
	I want to use the music metadata port
	So I can query for artist metadata
	I also want to ensure that the emulated and Http adapters both fulfil the same port contract

@Contract
Scenario Outline: a query that identifies a single artist returns that artist
	Given I am using the <Type> music metadata adapter
	When I query for 'Kate Bush'
	Then no error occurs
	And I get the following list of artists:
		| Name       | MbId                                 |
		| Bush, Kate | 4b585938-f271-45e2-b19a-91c634b5e396 |

Examples:
	| Type |
#	| Emulated |
	| Http |

@Contract
Scenario Outline: a query that identifies multiple artists returns them all
	Given I am using the <Type> music metadata adapter
	When I query for 'Bush'
	Then no error occurs
	And I get the following list of artists:
		| Name       | MbId                                 |
		| Bush       | 93ccd76c-3790-4435-a8bf-02bc26294b93 |
		| Bush, Kate | 4b585938-f271-45e2-b19a-91c634b5e396 |

Examples:
	| Type |
#	| Emulated |
	| Http |

@Contract
Scenario Outline: a query that identifies no artists returns an empty list
	Given I am using the <Type> music metadata adapter
	When I query for 'laiue4wriopuqy3'
	Then no error occurs
	And I get no results

	Examples:
	| Type |
#	| Emulated |
	| Http |

@Contract
Scenario Outline: a query for a name that includes lucene special characters works correctly
	Given I am using the <Type> music metadata adapter
	When I query for ':'
	Then no error occurs
	And I get the following list of artists:
		| Name           | MbId                                 |
		| phase : : vier | 29a96e55-2793-415b-8d36-947a1f57f535 |

Examples:
	| Type |
#	| Emulated |
	| Http |

@Integration
Scenario: a query that is repeated faster than the rate limit succeeds
	Given I am using the Http music metadata adapter
	When I query for 'Kate Bush' 20 times in a row
	Then no error occurs
