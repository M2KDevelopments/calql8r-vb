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
Namespace calql8r.OperatorsExtra

	''' 
	''' <summary>
	''' @author MARTIN
	''' </summary>
	Public Class CalTime
		Inherits GeneralCalButton

		Private hr, min, sec As Double

		Public Overrides Function displayText() As String
			Return "⁰"
		End Function

        Public Overridable WriteOnly Property Hours As GeneralCalValue
            Set(ByVal hours As GeneralCalValue)
                hr = hours.my_value
            End Set
        End Property

        Public Overridable WriteOnly Property Minutes As GeneralCalValue
            Set(ByVal minutes As GeneralCalValue)
                min = minutes.my_value
            End Set
        End Property

        Public Overridable WriteOnly Property Seconds As GeneralCalValue
            Set(ByVal seconds As GeneralCalValue)
                sec = seconds.my_value
            End Set
        End Property

		Public Overridable Function calculate() As GeneralCalValue

			'carry of the 60
			Const HR_MIN_SEC As Integer = 60
			min += CInt(Math.Truncate(sec / HR_MIN_SEC)) 'add 60 seconds as a minutes
			sec = sec Mod HR_MIN_SEC

			hr += CInt(Math.Truncate(min / HR_MIN_SEC)) 'add 60 minuutes as an hour
			min = (min Mod HR_MIN_SEC) 'set remainder as minutes


			'fix decimals in values

			'hour        
			min += (hr - CInt(Math.Truncate(hr))) * HR_MIN_SEC 'convert and add decimals to minutes
			hr = CInt(Math.Truncate(hr))

			'minutes
			sec += (min - CInt(Math.Truncate(min))) * HR_MIN_SEC 'convert and add decimals to seconds
			min = CInt(Math.Truncate(min))

			'convert to one value
			Dim time_value As Double = (hr) + (min/HR_MIN_SEC) + (sec/(HR_MIN_SEC*HR_MIN_SEC))
			Return New GeneralCalValue(time_value)
		End Function
	End Class

End Namespace