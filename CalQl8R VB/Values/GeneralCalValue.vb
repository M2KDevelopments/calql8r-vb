'========================================================================
' This conversion was produced by the Free Edition of
' Java to VB Converter courtesy of Tangible Software Solutions.
' Order the Premium Edition at https://www.tangiblesoftwaresolutions.com
'========================================================================



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
	Public Class GeneralCalValue
		Inherits GeneralCalButton

        Protected Friend my_value As Double

        Public Sub New()
            Me.my_value = 0
        End Sub

        Public Sub New(ByVal value As Double)
            Me.my_value = value
        End Sub

        Public Overrides Function displayText() As String
            Return "#GV (" & my_value & ")"
        End Function
        Public Overridable Property Value As Double
            Get
                Return my_value
            End Get
            Set(ByVal this_value As Double)
                Me.my_value = this_value
            End Set
        End Property


	End Class

End Namespace