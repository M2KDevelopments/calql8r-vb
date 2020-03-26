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
	''' @author MARTIN
	''' </summary>
	Public Class CalRandom
		Inherits GeneralCalValue

		'Constructor create a three-valued random decimal number e.g 0.142
		Public Sub New()
		   Dim rnd As New Random(CInt(Math.Truncate(GlobalRandom.NextDouble * 7000)))
		   Dim a As Integer = rnd.Next(100)
'JAVA TO VB CONVERTER WARNING: Java to VB Converter cannot determine whether both operands of this division are integer types - if they are then you should use the VB integer division operator:
		   Dim b As Integer = rnd.Next(100)/10
'JAVA TO VB CONVERTER WARNING: Java to VB Converter cannot determine whether both operands of this division are integer types - if they are then you should use the VB integer division operator:
		   Dim c As Integer = rnd.Next(100)/100
            my_value = (a + b + c) / 100.0
		End Sub

		Public Overrides Function displayText() As String
		   Return "#Rand"
		End Function

	End Class

End Namespace