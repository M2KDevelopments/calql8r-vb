'----------------------------------------------------------------------------------------------------------
'	Copyright © 2007 - 2018 Tangible Software Solutions Inc.
'	This module can be used by anyone provided that the copyright notice remains intact.
'
'	This module is used to replace calls to the static java.lang.Math.random method.
'----------------------------------------------------------------------------------------------------------
Friend Module GlobalRandom
	Private randomInstance As System.Random = Nothing

	Friend ReadOnly Property NextDouble() As Double
		Get
			If randomInstance Is Nothing Then
				randomInstance = New System.Random()
			End If

			Return randomInstance.NextDouble()
		End Get
	End Property
End Module