//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;

public interface UTConfigurableProperty
{
	string Name { get; set; }

	string Value { get; set; }

	string Expression{ get; set; }

	bool UseExpression { get; set; }

	bool IsMachineSpecific{ get; set; }

	bool IsPrivate { get; set; }
	
	bool SupportsPrivate {get;}
}

