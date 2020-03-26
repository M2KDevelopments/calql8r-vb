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
	Public Class CalCombinations
		Inherits GeneralCalOperator


		Public Overrides Function calculate() As GeneralCalValue
            Dim bothNumbersAreInts As Boolean = Not numberIsDecimal(number_1) AndAlso Not numberIsDecimal(number_2)
            If bothNumbersAreInts Then
                Dim n As Long = factorial(CInt(Math.Truncate(number_1)))
                Dim r As Long = factorial(CInt(Math.Truncate(number_2)))
                Dim n_r As Long = factorial(CInt(Math.Truncate(number_1)) - CInt(Math.Truncate(number_2)))
                Dim c As Double = CDbl(n) / ((n_r) * r)

                'number are to big for arithematic operations
                If c < 0 Then
                    Console.Error.WriteLine("Error...0x2100 Combinations numbers are to big for arithematic operations")
                    Return mathError()
                End If
                Return New GeneralCalValue(c)
            Else
                Console.Error.WriteLine("Error...0x2000 Combinations numbers have to be intergers")
                Return mathError()
            End If
		End Function

		Public Overrides Function displayText() As String
			Return "C"
		End Function

        Public Overrides Function getCalObjType() As Enums.EnumCalObjs
            Return Enums.EnumCalObjs.CAL_COMBINATION
        End Function

		Public Overrides Function usesOneNumber() As Boolean
	 Return False
		End Function

	End Class

End Namespace