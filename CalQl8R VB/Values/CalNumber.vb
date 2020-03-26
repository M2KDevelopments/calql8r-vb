'========================================================================
' This conversion was produced by the Free Edition of
' Java to VB Converter courtesy of Tangible Software Solutions.
' Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
'========================================================================

Imports System

'
' * To change this license header, choose License Headers in Project Properties.
' * To change this template file, choose Tools | Templates
' * and open the template in the editor.
' 
Namespace calql8r.Values

	''' 
	''' <summary>
	''' @author MARTIN KULULANGA
	''' </summary>
	Public Class CalNumber
		Inherits GeneralCalValue

		Public Sub New(ByVal num As Integer)
            my_value = num
        End Sub

        Public Overrides Function displayText() As String
            Return CInt(Math.Truncate(my_value)).ToString()
        End Function
	End Class

End Namespace