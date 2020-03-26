Imports CalQl8R_VB.calql8r.Values

'
' * To change this license header, choose License Headers in Project Properties.
' * To change this template file, choose Tools | Templates
' * and open the template in the editor.
' 
Namespace calql8r

    ''' 
    ''' <summary>
    ''' @author MARTIN
    ''' </summary>
    Public MustInherit Class GeneralCalButton

        Public Const TYP_FUNCTION As Integer = 1
        Public Const TYP_OPERATOR As Integer = 2
        Public Const EXTRA_OPERATOR As Integer = 3
        Public Const LOGIC As Integer = 4
        Public Const VALUES As Integer = 5

        Public MustOverride Function displayText() As String

        Public Overrides Function ToString() As String
            Return displayText()
        End Function

        Protected Friend Overridable Function mathError() As CalAns
            Dim a As New CalAns(0)
            a.ErrorType = Enums.EnumError.MATH
            Return a
        End Function

        Protected Friend Overridable Function syntaxError() As CalAns
            Dim a As New CalAns(0)
            a.ErrorType = Enums.EnumError.SYNTAX
            Return a
        End Function
    End Class

End Namespace