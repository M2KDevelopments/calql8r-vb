'========================================================================
' This conversion was produced by the Free Edition of
' Java to VB Converter courtesy of Tangible Software Solutions.
' Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
'========================================================================

Imports System

Imports CalQl8R_VB.calql8r.Values


'
' * To change this license header, choose License Headers in Project Properties.
' * To change this template file, choose Tools | Templates
' * and open the template in the editor.
' 
Namespace calql8r.Operators

	''' 
	''' <summary>
	''' @author MARTIN
	''' </summary>
	Public Class CalFactorial
		Inherits GeneralCalOperator


		Public Overrides Function calculate() As GeneralCalValue
            If numberIsDecimal(number_1) Then
                Console.Error.WriteLine("Error...0x4000 Factorial number is not an integer.")
                Return mathError()

            Else
                Dim fc As Long = factorial(CInt(Math.Truncate(number_1)))
                Return New GeneralCalValue(CDbl(fc))
            End If
		End Function

		Public Overrides Function displayText() As String
			Return "!"
		End Function

        Public Overrides Function getCalObjType() As Enums.EnumCalObjs
            Return Enums.EnumCalObjs.CAL_FACTORIAL
        End Function

		Public Overrides Function usesOneNumber() As Boolean
			Return True
		End Function

	End Class

End Namespace