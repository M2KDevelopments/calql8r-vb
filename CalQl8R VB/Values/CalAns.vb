Imports System
Imports Microsoft.VisualBasic
Imports CalQl8R_VB.calql8r.Enums

Namespace calql8r.Values

    ''' 
    ''' <summary>
    ''' @author MARTIN
    ''' </summary>
    Public Class CalAns
        Inherits GeneralCalValue

        Private answer_error As Enums.EnumError
        Private answerFormat As Enums.EnumAnswerFormat
        Private ReadOnly answer_fraction, answer_time As String

        Public Sub New()
            answer_error = Enums.EnumError.NONE
            answerFormat = Enums.EnumAnswerFormat.VALUE
            answer_fraction = "0"
            answer_time = "0⁰0⁰0⁰"
        End Sub

        Public Sub New(ByVal ans As Double)
            my_value = ans
            answer_error = Enums.EnumError.NONE
            answerFormat = Enums.EnumAnswerFormat.VALUE

            If ans <> 0 Then
                answer_fraction = calculateFractionFormat(ans)
                answer_time = calculateTimeFormat(ans)
            Else
                answer_time = "0⁰0⁰0⁰"
                answer_fraction = "0"
            End If

        End Sub

        Public Overrides Function displayText() As String
            Return "Ans"
        End Function

        Public Overridable Property ErrorType As Enums.EnumError
            Get
                Return answer_error
            End Get
            Set(ByVal value As Enums.EnumError)
                Me.answer_error = value
            End Set
        End Property


        Public Overridable Property AnswerFormatType As Enums.EnumAnswerFormat
            Get
                Return Me.answerFormat
            End Get
            Set(ByVal value As Enums.EnumAnswerFormat)
                Me.answerFormat = value
            End Set
        End Property


        Public Overridable Overloads Function getValue(ByVal number_of_dps As Integer) As Double

            'convert double to string
            Dim value_str As String = Me.my_value.ToString()

            'locate decimal point
            Dim dp As Integer = value_str.IndexOf(".", StringComparison.Ordinal)

            Try
                If dp > 0 Then
                    'truncate the string
                    value_str = value_str.Substring(0, dp + number_of_dps + 1)

                    'convert back to double
                    Dim new_value As Double = Double.Parse(value_str)

                    Return new_value
                End If

            Catch ex As System.FormatException
                Console.Error.WriteLine("Error...0x14" & vbLf & "CalAns::GetValue Method Error" & vbLf & "Number format could not be parse")
                Return my_value
            Catch ex As Exception 'numbers of dps > substring length
                Console.WriteLine("Error...0x15" & vbLf & "CalAns::GetValue Method Error" & vbLf & "Number decimals Places greater than value")
                Return my_value
            End Try
            Return my_value
        End Function

        Private Function calculateFractionFormat(ByVal ans As Double) As String

            Dim whole As Integer = CInt(Math.Truncate(ans))
            Dim number_is_negative As Boolean = (whole < 0)
            If number_is_negative Then
                whole *= -1
            End If

            'get decimal part
            Dim [decimal] As Double = Math.Abs(ans) - Math.Abs(whole)
            Dim numerator As Integer = -1, denomerator As Integer = -1

            'number is already a integer
            If [decimal] = 0 Then
                Return "" & ans
            End If

            'multiple value and see if it rurns into a whole number
            For i As Integer = 1 To 999

                ' its a whole number
                Dim n1 As Double = [decimal] * i
                Dim n2 As Integer = CInt(Math.Truncate(n1))
                Dim number_is_an_integer As Boolean = ((n1 - n2) = 0)
                If number_is_an_integer Then
                    denomerator = i
                    numerator = n2
                    Exit For
                End If
            Next i

            'check if numerator and denomator where found
            If numerator = -1 OrElse denomerator = -1 Then
                Return ans.ToString()
            Else
                Dim fraction As String = numerator & "/" & denomerator
                Dim whole_number As String = ""
                If whole > 0 Then
                    If number_is_negative Then
                        whole *= -1
                    End If
                    whole_number = whole & "/"
                End If
                Return whole_number & fraction
            End If
        End Function

        Private Function calculateTimeFormat(ByVal ans As Double) As String
            Const HR_MIN_SEC As Integer = 60
            'convert to time format
            If ans > 0 Then
                Dim hr As Integer = CInt(Math.Truncate(ans))
                Dim min As Double = ((ans - CInt(Math.Truncate(ans))) * HR_MIN_SEC)
                Dim sec As Double = (min - CInt(Math.Truncate(min))) * HR_MIN_SEC
                Return hr & "⁰" & CInt(Math.Truncate(min)) & "⁰" & CInt(Math.Truncate(sec)) & "⁰"
            Else
                Return "" & ans
            End If
        End Function

        Public Overridable ReadOnly Property GetAnswerFraction As String
            Get
                Return answer_fraction
            End Get
        End Property

        Public Overridable ReadOnly Property GetAnswerTime As String
            Get
                Return answer_time
            End Get
        End Property

    End Class

End Namespace