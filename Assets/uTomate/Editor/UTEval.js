//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

/**
 * Simple, but effective hack to allow evaluation of arbitrary expressions
 * in any uTomate action.
 */
public static function Evaluate(string:String, context:Object) : Object {
 	try {
    	return eval(string);
	}
	catch(e) {
	    // show the error message here because outside we will just get an InvocationTargetException with 
	    // no information at all.
		Debug.LogError(e);
		throw e;
	}
}

// This is for suppressing the silly warnings coming up in MonoDevelop for not using 
// UnityEditor and System.Collections namespaces. We didn't even import them but hell...
private class SuppressWarnings {var q:Queue; var h:Help; var g:GUI;}