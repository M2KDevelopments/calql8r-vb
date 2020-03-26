
Imports CalQl8R_VB.calql8r.Values
Imports CalQl8R_VB.calql8r.Enums.EnumCalObjs



Namespace calql8r.Operators

	''' 
	''' <summary>
	''' @author MARTIN
	''' </summary>
	Public Class CalDivision
		Inherits GeneralCalOperator


		Public Overrides Function calculate() As GeneralCalValue
            Dim sln As Double = number_1 / number_2
			Dim value As New GeneralCalValue(sln)
			Return value
		End Function

		Public Overrides Function displayText() As String
			Return "÷"
		End Function

        Public Overrides Function getCalObjType() As Enums.EnumCalObjs
            Return Enums.EnumCalObjs.CAL_DIVISION
        End Function

		Public Overrides Function usesOneNumber() As Boolean
			Return False
		End Function

	End Class

End Namespace