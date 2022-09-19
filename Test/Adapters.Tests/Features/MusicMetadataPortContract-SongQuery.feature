Feature: MusicMetadataPortContract-SongQuery
	As a developer
	I want to use the music metadata port
	So I can query for song metadata
	I also want to ensure that the emulated and Http adapters both fulfil the same port contract

@Contract
Scenario Outline: a query that requests all the songs for an artist
	Given I am using the <Type> music metadata adapter
	When I query for the songs of 'Kate Bush:4b585938-f271-45e2-b19a-91c634b5e396'
	Then no error occurs
	And I get a list of 148 songs including:
		| Title         | MbId                                 |
		| Cloudbusting  | 0088e966-eaa4-37e0-8a04-2c2c8372e649 |
		| Experiment IV | f88bd645-ef6a-3d99-ba02-1e542df2de79 |

Examples:
	| Type |
#	| Emulated |
	| Http |

