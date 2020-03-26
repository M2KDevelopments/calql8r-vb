'========================================================================
' This conversion was produced by the Free Edition of
' Java to VB Converter courtesy of Tangible Software Solutions.
' Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
'========================================================================


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
	Public Class CalModulus
		Inherits GeneralCalOperator


		Public Overrides Function calculate() As GeneralCalValue
            Dim sln As Double = number_1 Mod number_2
			Dim value As New GeneralCalValue(sln)
			Return value
		End Function

		Public Overrides Function displayText() As String
			 Return "MOD"
		End Function

        Public Overrides Function getCalObjType() As Enums.EnumCalObjs
            Return Enums.EnumCalObjs.CAL_MODULUS
        End Function

		Public Overrides Function usesOneNumber() As Boolean
	 Return False
		End Function

	End Class
End Namespace