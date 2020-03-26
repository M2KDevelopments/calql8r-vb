Imports System
Imports CalQl8R_VB.calql8r.Enums.EnumCalObjs
Imports CalQl8R_VB.calql8r.Values


Namespace calql8r.Operators

	''' 
	''' <summary>
	''' @author MARTIN
	''' </summary>
	Public MustInherit Class GeneralCalOperator
		Inherits GeneralCalButton

        Protected Friend number_1, number_2 As Double
        Public MustOverride Function calculate() As GeneralCalValue

        Public MustOverride Overrides Function displayText() As String
        Public MustOverride Function usesOneNumber() As Boolean
        Public MustOverride Function getCalObjType() As Enums.EnumCalObjs

        Protected Friend Overridable Function numberIsDecimal(ByVal num As Double) As Boolean
            'if the truncated number minus number is not 0. Then is it has decimal numbers
            Return (CInt(Math.Truncate(num))) - num <> 0
        End Function

        Protected Friend Overridable Function factorial(ByVal n As Integer) As Long
            Dim fc As Long = 1
            If n = 0 Then
                Return 1
            ElseIf n < 0 Then
                fc *= -1
            End If
            For i As Integer = n To 1 Step -1
                fc *= i
            Next i
            Return fc
        End Function

        Public Overridable Property Number1 As Double
            Get
                Return number_1
            End Get
            Set(ByVal value As Double)
                Me.number_1 = value
            End Set
        End Property


        Public Overridable Property Number2 As Double
            Get
                Return number_2
            End Get
            Set(ByVal value As Double)
                Me.number_2 = value
            End Set
        End Property


    End Class

End Namespace