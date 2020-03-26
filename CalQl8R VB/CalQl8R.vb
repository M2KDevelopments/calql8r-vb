Imports System
Imports System.Collections.Generic
Imports System.Threading
Imports Microsoft.VisualBasic
Imports CalQl8R_VB.calql8r.Enums
Imports CalQl8R_VB.calql8r.Functions.GeneralCalFunction
Imports CalQl8R_VB.calql8r.Values
Imports CalQl8R_VB.calql8r.Logic
Imports CalQl8R_VB.calql8r.Operators.CalAdd
Imports CalQl8R_VB.calql8r.Operators.CalBase10
Imports CalQl8R_VB.calql8r.Operators.CalFactorial
Imports CalQl8R_VB.calql8r.Operators.CalPow
Imports CalQl8R_VB.calql8r.Operators.CalSubtract
Imports CalQl8R_VB.calql8r.Operators.GeneralCalOperator
Imports CalQl8R_VB.calql8r.OperatorsExtra.CalTime
Imports CalQl8R_VB.calql8r.Operators
Imports CalQl8R_VB.calql8r.OperatorsExtra
Imports CalQl8R_VB.calql8r.Functions

Namespace calql8r

    ''' <summary>
    ''' @author MARTIN
    ''' </summary>
    Public NotInheritable Class CalQl8R

        Private calObjs As List(Of GeneralCalButton)
        Private answer As CalAns

        'CONSTRUCTORS
        Public Sub New()
            calObjs = New List(Of GeneralCalButton)()
            answer = New CalAns(0)
            calObjs.Clear()
        End Sub

        ''' <param name="expression"> The expression to be calculated. For example: 1+3/4*5-(42-24). </param>
        Public Sub New(ByVal expression As String)
            calObjs = New List(Of GeneralCalButton)()
            answer = New CalAns(0)
            calObjs.Clear()
            expression = expression
        End Sub

        Public Sub run()
            calObjs = decimalLoop(calObjs)
            calObjs = numberLoop(calObjs)
            calObjs = timeLoop(calObjs)
            calObjs = positiveAndNegativeLoop(calObjs)
            calObjs = bracketAbsLoop(calObjs)
            calObjs = functionLoop(calObjs)
            answer = CType(calculateSolution(calObjs), CalAns)
        End Sub

        Private Function calculateSolution(ByVal calculatorObjs As List(Of GeneralCalButton)) As GeneralCalValue
            calculatorObjs = decimalLoop(calculatorObjs)
            calculatorObjs = numberLoop(calculatorObjs)
            calculatorObjs = positiveAndNegativeLoop(calculatorObjs)
            calculatorObjs = bracketLoop(calculatorObjs)
            calculatorObjs = positiveAndNegativeLoop(calculatorObjs)
            Return operatorLoop(calculatorObjs)
        End Function

        Private Function calculateSolution(ByVal ParamArray calculatorObjs() As CalQl8R_VB.calql8r.GeneralCalButton)

            Dim calculatorObjsList As New List(Of GeneralCalButton)()
            For i As Integer = 0 To calculatorObjs.Count - 1
                calculatorObjsList.Add(calculatorObjs(i))
            Next
            calculatorObjsList = functionLoop(calculatorObjsList)
            Return calculateSolution(calculatorObjsList)
        End Function

        Public Sub clearMemory()
            calObjs.Clear()
        End Sub

        ''' <summary>
        ''' This method finds the preceeding and proceeding number from the decimal point and combines them. </summary>
        ''' <param name="calculatorObj">
        ''' @return </param>
        Private Function decimalLoop(ByVal calculatorObj As List(Of GeneralCalButton)) As List(Of GeneralCalButton)

            Try
                Dim searchingIndex As Integer = 0
                Do While searchingIndex < calculatorObj.Count

                    If TypeOf calculatorObj(searchingIndex) Is CalDecimal Then

                        'find preceeding numbers
                        Dim preceedIndex As Integer = searchingIndex - 1
                        Do While TypeOf calculatorObj(preceedIndex) Is CalNumber
                            preceedIndex -= 1
                            If preceedIndex = -1 Then
                                preceedIndex = 0
                                Exit Do
                            End If
                        Loop

                        'find proceeding numbers
                        Dim proceedIndex As Integer = searchingIndex + 1
                        Do While TypeOf calculatorObj(proceedIndex) Is CalNumber
                            proceedIndex += 1
                            If proceedIndex = calculatorObj.Count Then
                                proceedIndex -= 1
                                Exit Do
                            End If
                        Loop

                        'fix the indices to corrct first and last values
                        If preceedIndex > 0 Then
                            preceedIndex += 1
                        ElseIf Not (TypeOf calculatorObj(0) Is CalNumber) Then
                            preceedIndex += 1
                        End If
                        If proceedIndex < (calculatorObj.Count - 1) Then
                            proceedIndex -= 1
                        ElseIf Not (TypeOf calculatorObj(calculatorObj.Count - 1) Is CalNumber) Then
                            proceedIndex -= 1
                        End If

                        'create the preceeding and proceeding numbers
                        Dim preceedingNumber As Double = 0
                        Dim proceedingNumber As Double = 0
                        Const BASE As Integer = 10
                        For i As Integer = preceedIndex To searchingIndex - 1
                            Dim gv As GeneralCalValue = CType(calculatorObj(i), GeneralCalValue)
                            preceedingNumber *= BASE
                            preceedingNumber += gv.my_value
                        Next i

                        Dim pow As Integer = 0
                        Try
                            For i As Integer = searchingIndex + 1 To proceedIndex
                                Dim gv As GeneralCalValue = CType(calculatorObj(i), GeneralCalValue)
                                pow += 1
                                'JAVA TO VB CONVERTER WARNING: An assignment within expression was extracted from the following statement:
                                'ORIGINAL LINE: proceedingNumber += gv.getValue() / Math.pow(BASE, ++pow);
                                'JAVA TO VB CONVERTER WARNING: Java to VB Converter cannot determine whether both operands of this division are integer types - if they are then you should use the VB integer division operator:
                                proceedingNumber += gv.my_value / Math.Pow(BASE, pow)
                            Next i
                        Catch ex As Exception
                            Console.Error.WriteLine("Error...0x01" & vbLf & "decimal loop error:proceeding number" & vbLf & ex.Message)

                        End Try

                        'replace the objects
                        Dim number_of_object As Integer = proceedIndex - preceedIndex
                        calculatorObj.Insert(preceedIndex, New GeneralCalValue(preceedingNumber + proceedingNumber))

                        'removing
                        For i As Integer = 0 To number_of_object
                            Dim del As Integer = preceedIndex + 1
                            calculatorObj.RemoveAt(del)
                        Next i

                        'reset searching 
                        searchingIndex = -1
                    End If

                    'contiune or break the searching loop
                    searchingIndex += 1
                Loop

                Return calculatorObj
            Catch ex As Exception
                Console.Error.WriteLine("Error...0x01" & vbLf & "decimal loop error" & vbLf & ex.Message)
            End Try
            Return Nothing
        End Function

        ''' <summary>
        ''' Convert adjacent one object numbers into one general value number </summary>
        ''' <param name="calculatorObj">
        ''' @return </param>
        Private Function numberLoop(ByVal calculatorObj As List(Of GeneralCalButton)) As List(Of GeneralCalButton)
            Try
                Dim searchingIndex As Integer = 0
                Do While searchingIndex < calculatorObj.Count
                    If TypeOf calculatorObj(searchingIndex) Is CalNumber Then

                        'count adjacent numbers
                        Dim last_number As Integer = searchingIndex
                        Do While TypeOf calculatorObj(last_number) Is CalNumber
                            last_number += 1
                            If last_number = calculatorObj.Count Then
                                Exit Do
                            End If
                        Loop

                        Dim value As Double = 0
                        Const BASE As Integer = 10
                        For i As Integer = searchingIndex To last_number - 1
                            value *= BASE
                            value += CType(calculatorObj(i), CalNumber).my_value
                        Next i

                        calculatorObj.Insert(searchingIndex, New GeneralCalValue(value))
                        'delete existing CalNumbers
                        For i As Integer = searchingIndex + 1 To last_number
                            Dim del As Integer = searchingIndex + 1
                            calculatorObj.RemoveAt(del)
                        Next i

                        'reset search loop
                        searchingIndex = 0
                        Continue Do
                    End If

                    searchingIndex += 1
                Loop

                Return calculatorObj
            Catch ex As Exception
                Console.Error.WriteLine("Error...0x2" & vbLf & "number loop error" & vbLf & ex.Message)
            End Try
            Return Nothing
        End Function

        ''' <summary>
        ''' This method finds an operator and calculate the value from left and right values. </summary>
        ''' <param name="calculatorObj">
        ''' @return </param>
        Private Function operatorLoop(ByVal calculatorObj As List(Of GeneralCalButton)) As CalAns
            Try

                'loop through emumeration of operators to mimic bodmas
                For Each math_operator As Enums.EnumCalObjs In Enums.EnumCalObjs.values()

                    Dim searchingIndex As Integer = 0
                    Do While searchingIndex < calculatorObj.Count
                        If TypeOf calculatorObj(searchingIndex) Is GeneralCalOperator Then
                            Dim opr As GeneralCalOperator = (CType(calculatorObj(searchingIndex), GeneralCalOperator))
                            If opr.getCalObjType Is math_operator Then

                                Try
                                    'get and calculate values
                                    Dim gv1 As GeneralCalValue = CType(calculatorObj(searchingIndex - 1), GeneralCalValue)
                                    If Not opr.usesOneNumber() Then

                                        Dim gv2 As GeneralCalValue = CType(calculatorObj(searchingIndex + 1), GeneralCalValue)

                                        opr.Number1 = gv1.Value
                                        opr.Number2 = gv2.Value
                                    Else

                                        opr.Number1 = gv1.Value
                                        opr.Number2 = gv1.Value
                                    End If

                                    Dim gv As GeneralCalValue = opr.calculate()

                                    If TypeOf gv Is CalAns Then
                                        Console.Error.WriteLine("Error...0x3000 Calculation Error")
                                        Return CType(gv, CalAns)
                                    End If

                                    'replace expression
                                    calculatorObj.Insert(searchingIndex - 1, gv)
                                    calculatorObj.RemoveAt(searchingIndex) 'num1
                                    calculatorObj.RemoveAt(searchingIndex) 'operator
                                    If Not opr.usesOneNumber() Then
                                        calculatorObj.RemoveAt(searchingIndex) 'num2
                                    End If

                                    'reset search
                                    searchingIndex = -1

                                Catch ex As Exception
                                    Console.Error.WriteLine("Error...0x4" & vbLf & "operator loop error" & vbLf & "calculatorObj.size()=" & calculatorObj.Count)
                                    Dim ans As New CalAns()
                                    ans.ErrorType = Enums.EnumError.SYNTAX
                                    Return ans
                                End Try
                            End If

                        End If
                        searchingIndex += 1
                    Loop
                Next math_operator
                Try
                    Dim ans As Double = CType(calculatorObj(0), GeneralCalValue).my_value
                    Return New CalAns(ans)
                Catch ex As Exception
                    Console.Error.WriteLine("Error...0x3" & vbLf & "operator loop error: last object not an answer" & vbLf & ex.Message)
                End Try

            Catch ex As Exception
                Console.Error.WriteLine("Error...0x3" & vbLf & "operator loop error" & vbLf & ex.Message)
            End Try
            Dim a As New CalAns(0)
            a.ErrorType = Enums.EnumError.SYNTAX
            Return a
        End Function

        ''' <summary>
        ''' This method finds the inner most bracket and calculate the solution to the expression </summary>
        ''' <param name="calculatorObj">
        ''' @return </param>
        Private Function bracketLoop(ByVal calculatorObj As List(Of GeneralCalButton)) As List(Of GeneralCalButton)
            Try

                Dim searching As Integer = 0
                Do While searching < calculatorObj.Count
                    If TypeOf calculatorObj(searching) Is CalBracketOpen Then
                        Const NONE As Integer = -1
                        Dim last_opening_bracket As Integer = NONE
                        Dim first_closing_bracket As Integer = NONE

                        'get index of innermost opening bracket
                        For i As Integer = 0 To calculatorObj.Count - 1
                            If TypeOf calculatorObj(i) Is CalBracketOpen Then
                                last_opening_bracket = i
                            End If
                        Next i

                        'get index of first closing bracket
                        Dim closing_bracket_search As Integer = last_opening_bracket
                        Do While closing_bracket_search < calculatorObj.Count

                            If TypeOf calculatorObj(closing_bracket_search) Is CalBracketClose Then
                                first_closing_bracket = closing_bracket_search

                                'break this local loop
                                closing_bracket_search = calculatorObj.Count
                            End If
                            closing_bracket_search += 1
                        Loop

                        'check if brackets are in pairs
                        If (first_closing_bracket <> NONE) AndAlso (last_opening_bracket = NONE) Then
                            Console.Error.WriteLine("Error...0x06" & vbLf & "Bracket Loop Error: No Opening Bracket" & vbLf)
                            Return Nothing
                        End If
                        'check if brackets are NOT ) (
                        If (first_closing_bracket < last_opening_bracket) Then
                            Console.Error.WriteLine("Error...0x07" & vbLf & "Bracket Loop Error: No Opening Bracket" & vbLf)
                            Return Nothing
                        End If
                        'check if brackets are in pairs
                        If (first_closing_bracket = NONE) AndAlso (last_opening_bracket <> NONE) Then
                            Console.Error.WriteLine("Error...0x08" & vbLf & "Bracket Loop Error: No Closing Bracket" & vbLf)
                            Return Nothing
                        End If

                        'get the expression inside the bracket
                        Dim mini_expression As New List(Of GeneralCalButton)()
                        mini_expression.Clear()
                        For i As Integer = last_opening_bracket + 1 To first_closing_bracket - 1
                            mini_expression.Add(calculatorObj(i))
                        Next i

                        'calculate solution of the expression inside the bracket
                        Dim gv As GeneralCalValue = CType(operatorLoop(mini_expression), GeneralCalValue)

                        'replace brackets and exprssion with general value object
                        calculatorObj.Insert(last_opening_bracket, gv)
                        Dim number_of_items As Integer = first_closing_bracket - last_opening_bracket
                        'removing objects
                        For i As Integer = 0 To number_of_items
                            Dim del As Integer = last_opening_bracket + 1
                            calculatorObj.RemoveAt(del)
                        Next i

                        'reset loop
                        searching = -1
                    End If
                    searching += 1
                Loop

                Return calculatorObj
            Catch ex As Exception
                Console.Error.WriteLine("Error...0x05" & vbLf & "Bracket Loop Error" & vbLf & ex.Message)
            End Try
            Return Nothing
        End Function

        ''' <summary>
        ''' This method finds repeating minus and addition signs and replaces them </summary>
        ''' <param name="calculatorObj">
        ''' @return </param>
        Private Function positiveAndNegativeLoop(ByVal calculatorObj As List(Of GeneralCalButton)) As List(Of GeneralCalButton)
            Try

                Dim searchingIndex As Integer = 0
                Do While searchingIndex < calculatorObj.Count - 1

                    If TypeOf calculatorObj(searchingIndex) Is CalSubtract AndAlso (searchingIndex <> calculatorObj.Count - 1) Then
                        Try
                            'is the subtract/minus is the first object
                            If searchingIndex = 0 AndAlso TypeOf calculatorObj(searchingIndex + 1) Is GeneralCalValue Then
                                Dim value As Double = CType(calculatorObj(searchingIndex + 1), GeneralCalValue).my_value
                                CType(calculatorObj(searchingIndex + 1), GeneralCalValue).my_value = -value
                                calculatorObj.RemoveAt(0)
                                Continue Do
                            End If

                            'if subtract poses as a minus 
                            Dim search_within_range As Boolean = searchingIndex > 0 AndAlso searchingIndex < calculatorObj.Count - 1

                            If search_within_range Then
                                Dim left_object_is_potential_value As Boolean = (TypeOf calculatorObj(searchingIndex - 1) Is GeneralCalValue) OrElse (TypeOf calculatorObj(searchingIndex - 1) Is CalBracketClose)


                                Dim right_object_is_value As Boolean = TypeOf calculatorObj(searchingIndex + 1) Is GeneralCalValue

                                If Not left_object_is_potential_value AndAlso right_object_is_value Then
                                    Dim value As Double = CType(calculatorObj(searchingIndex + 1), GeneralCalValue).my_value
                                    CType(calculatorObj(searchingIndex + 1), GeneralCalValue).my_value = -value
                                    calculatorObj.RemoveAt(searchingIndex)
                                End If 'if there are two minus objects
                            ElseIf (TypeOf calculatorObj(searchingIndex + 1) Is CalMinus) OrElse (TypeOf calculatorObj(searchingIndex + 1) Is CalSubtract) Then
                                calculatorObj.Insert(searchingIndex, New Operators.CalAdd())
                                calculatorObj.RemoveAt(searchingIndex + 1) 'minus
                                calculatorObj.RemoveAt(searchingIndex + 1) 'subtraction

                                'reset search loop
                                searchingIndex = -1
                            ElseIf TypeOf calculatorObj(searchingIndex + 1) Is Operators.CalAdd Then
                                calculatorObj.RemoveAt(searchingIndex + 1) 'remove addition
                                'reset search loop
                                searchingIndex = -1
                            End If

                        Catch ex As Exception
                            Console.Error.WriteLine("Error...0x10" & vbLf & "Positive and Negative Loop Error: No next object" & vbLf & ex.Message)
                        End Try

                    ElseIf TypeOf calculatorObj(searchingIndex) Is CalMinus Then
                        Try
                            'if there are two minus objects 
                            If (TypeOf calculatorObj(searchingIndex + 1) Is CalMinus) Then
                                calculatorObj.Insert(searchingIndex, New Operators.CalAdd())
                                calculatorObj.RemoveAt(searchingIndex + 1) 'minus
                                calculatorObj.RemoveAt(searchingIndex + 1) 'subtraction

                                'reset search loop
                                searchingIndex = -1

                            ElseIf (TypeOf calculatorObj(searchingIndex + 1) Is GeneralCalValue) Then
                                'remove minus and negate value
                                Dim value As Double = -CType(calculatorObj(searchingIndex + 1), GeneralCalValue).my_value
                                CType(calculatorObj(searchingIndex + 1), GeneralCalValue).my_value = value
                                calculatorObj.RemoveAt(searchingIndex)

                                'reset search loop
                                searchingIndex = -1
                            End If

                        Catch ex As Exception
                            Console.Error.WriteLine("Error..0x11" & vbLf & "Positive and Negative Loop Error: No next object" & ex.Message)
                        End Try

                    ElseIf TypeOf calculatorObj(searchingIndex) Is Operators.CalAdd AndAlso (searchingIndex <> calculatorObj.Count - 1) Then

                        If (TypeOf calculatorObj(searchingIndex + 1) Is CalMinus) OrElse (TypeOf calculatorObj(searchingIndex + 1) Is Operators.CalSubtract) Then
                            Try
                                calculatorObj.Insert(searchingIndex, New Operators.CalSubtract())
                                calculatorObj.RemoveAt(searchingIndex + 1) 'remove addition
                                calculatorObj.RemoveAt(searchingIndex + 1) 'remove negative

                                'reset search loop
                                searchingIndex = -1
                            Catch ex As Exception
                                Console.Error.WriteLine("Error..0x12" & vbLf & "Positive and Negative Loop Error: No next object" & ex.Message)
                            End Try

                        ElseIf (TypeOf calculatorObj(searchingIndex + 1) Is Operators.CalAdd) Then
                            Try
                                calculatorObj.RemoveAt(searchingIndex) 'remove one addition
                                'reset search loop
                                searchingIndex = -1
                            Catch ex As Exception
                                Console.Error.WriteLine("Error..0x13" & vbLf & "Positive and Negative Loop Error: No next object" & ex.Message)
                            End Try
                        End If
                    End If

                    'continue loop
                    searchingIndex += 1
                Loop
                Return calculatorObj
            Catch ex As Exception
                Console.Error.WriteLine("Error..0x09" & vbLf & "Positive and Negative Loop Error" & ex.Message)
            End Try
            Return Nothing
        End Function

        ''' <summary>
        ''' This methods finds the three time point objects and converts the adjacent time values into a single value </summary>
        ''' <param name="calculatorObjs">
        ''' @return </param>
        Private Function timeLoop(ByVal calculatorObjs As List(Of GeneralCalButton)) As List(Of GeneralCalButton)
            Try
                Dim searchingIndex As Integer = 0
                Dim number_of_found_time_points As Integer = 0
                Do While searchingIndex < calculatorObjs.Count

                    'if still count time points
                    If (searchingIndex = calculatorObjs.Count - 1) AndAlso (Not (TypeOf calculatorObjs(searchingIndex) Is CalTime)) AndAlso (number_of_found_time_points <> 2 AndAlso number_of_found_time_points <> 0) AndAlso calculatorObjs.Count > 1 Then
                        Console.Error.WriteLine("Error...0x19000 Time Loop Syntax")
                        Return Nothing
                    ElseIf TypeOf calculatorObjs(searchingIndex) Is CalTime Then
                        Dim calTime As CalTime = CType(calculatorObjs(searchingIndex), CalTime)
                        number_of_found_time_points += 1

                        'set preceed value
                        If TypeOf calculatorObjs(searchingIndex - 1) Is GeneralCalValue Then
                            Dim gv As GeneralCalValue = CType(calculatorObjs(searchingIndex - 1), GeneralCalValue)

                            'if the value is positive
                            If gv.my_value > 0 Then
                                If number_of_found_time_points = 1 Then
                                    calTime.Hours = gv
                                Else 'the object before the general value must be a time point
                                    If TypeOf calculatorObjs(searchingIndex - 2) Is CalTime Then

                                        If number_of_found_time_points = 2 Then
                                            calTime.Minutes = gv
                                        ElseIf number_of_found_time_points = 3 Then
                                            calTime.Seconds = gv

                                            'get calculated answer
                                            gv = calTime.calculate()

                                            'add calculated time
                                            Dim time_expression_length As Integer = 6 '#t #t #t
                                            calculatorObjs.Insert(searchingIndex - (time_expression_length - 1), gv) 'add answer

                                            'remove time expression
                                            For i As Integer = 1 To time_expression_length
                                                Dim del As Integer = searchingIndex - time_expression_length + 2
                                                calculatorObjs.RemoveAt(del)
                                            Next i

                                            'reset function
                                            searchingIndex = -1
                                            number_of_found_time_points = 0
                                        End If
                                    Else
                                        Console.Error.WriteLine("Error...0x16000 Time Loop Syntax Error Brackets")
                                        Return Nothing
                                    End If
                                End If
                            End If
                            'find and evaluate bracket expression then set value
                        ElseIf TypeOf calculatorObjs(searchingIndex - 1) Is CalBracketClose Then
                            Dim number_of_closed_brackets As Integer = 0
                            Dim number_of_open_brackets As Integer = 0
                            Dim start_index As Integer = -1
                            Dim end_index As Integer = searchingIndex - 1

                            'find indices to encapsulate brackets
                            For i As Integer = searchingIndex - 1 To 0 Step -1
                                If TypeOf calculatorObjs(i) Is CalBracketOpen Then
                                    number_of_open_brackets += 1
                                ElseIf TypeOf calculatorObjs(i) Is CalBracketClose Then
                                    number_of_closed_brackets += 1
                                End If
                                If number_of_closed_brackets = number_of_open_brackets Then
                                    start_index = i
                                    Exit For
                                End If
                            Next i

                            If start_index <> -1 Then
                                'calculate bracket expression
                                Dim local_expression As New List(Of GeneralCalButton)()

                                For i As Integer = start_index To end_index
                                    local_expression.Add(calculatorObjs(i)) 'add
                                Next i

                                'get local solution
                                Dim gv As GeneralCalValue = calculateSolution(local_expression)

                                'add value 
                                calculatorObjs.Insert(start_index, gv)

                                'remove bracket expression
                                For i As Integer = start_index To end_index
                                    Dim del As Integer = start_index + 1
                                    calculatorObjs.RemoveAt(del)
                                Next i

                                'reset function
                                searchingIndex = -1
                                number_of_found_time_points = 0
                                local_expression.Clear()

                            Else
                                Console.Error.WriteLine("Error...0x17000 Time Loop Syntax Error Brackets")
                                Return Nothing
                            End If

                        Else
                            Console.Error.WriteLine("Error...0x18000 Time Loop Syntax Error")
                            Return Nothing
                        End If
                    End If
                    searchingIndex += 1
                Loop

                Return calculatorObjs

            Catch ex As Exception
                Console.Error.WriteLine("Error...0x15000 Time Loop Error" & vbLf & ex.Message)
                Return Nothing
            End Try

        End Function

        ''' <summary>
        ''' This methods finds the inner most function object and its corresponding
        ''' parameters and evaluates the expression. </summary>
        ''' <param name="calculatorObj">
        ''' @return </param>
        Private Function functionLoop(ByVal calculatorObj As List(Of GeneralCalButton)) As List(Of GeneralCalButton)

            Dim searchingIndex As Integer = 0
            Dim firstOpenBracket As Integer = 0
            Dim lastClosingBracket As Integer = calculatorObj.Count - 1

            Try

                Do While searchingIndex <= lastClosingBracket
                    If TypeOf calculatorObj(searchingIndex) Is GeneralCalFunction Then

                        'if generalvalue is the next object
                        If TypeOf calculatorObj(searchingIndex + 1) Is GeneralCalValue Then

                            'get the proceed value and calculate the one parametic function
                            Dim fnx As GeneralCalFunction = CType(calculatorObj(searchingIndex), GeneralCalFunction)

                            'if value value is being amplified by power, factorial or base
                            If searchingIndex + 3 <= calculatorObj.Count - 1 Then

                                'if the next object is amplifying the number
                                If TypeOf calculatorObj(searchingIndex + 2) Is Operators.CalPow OrElse TypeOf calculatorObj(searchingIndex + 2) Is Operators.CalFactorial OrElse TypeOf calculatorObj(searchingIndex + 2) Is CalBase10 Then

                                    'if the very next object is a value
                                    If TypeOf calculatorObj(searchingIndex + 3) Is GeneralCalValue Then

                                        'evaluate for single value
                                        Dim opr As GeneralCalOperator = CType(calculatorObj(searchingIndex + 2), GeneralCalOperator)
                                        Dim value1 As GeneralCalValue = CType(calculatorObj(searchingIndex + 1), GeneralCalValue)
                                        Dim value2 As GeneralCalValue = CType(calculatorObj(searchingIndex + 3), GeneralCalValue)
                                        opr.Number1 = value1.Value
                                        opr.Number2 = value2.Value

                                        Try
                                            'calculate
                                            Dim gv As GeneralCalValue = opr.calculate()

                                            'replace values and objects
                                            calculatorObj.Insert(searchingIndex + 1, gv)
                                            calculatorObj.RemoveAt(searchingIndex + 2) 'remove v1
                                            calculatorObj.RemoveAt(searchingIndex + 2) 'remove operator that amplifies number
                                            calculatorObj.RemoveAt(searchingIndex + 2) 'remove v2
                                        Catch ex As Exception
                                            Console.Error.WriteLine("Error...0x14000 Function Loop Operator Calculations" & vbLf & ex.Message)
                                            Return Nothing
                                        End Try

                                    End If
                                End If
                            End If

                            'add parameter to function
                            fnx.addParameter(CType(calculatorObj(searchingIndex + 1), GeneralCalValue))

                            If fnx.parameterCountIsOK() Then

                                Try

                                    Dim value As GeneralCalValue = fnx.calculate()

                                    calculatorObj.Insert(searchingIndex, value)

                                    calculatorObj.RemoveAt(searchingIndex + 1) 'remove the function object

                                    calculatorObj.RemoveAt(searchingIndex + 1) 'remove the value

                                Catch ex As Exception
                                    'return syntax error
                                    Console.Error.WriteLine("Error...0x13000 Function Loop Error" & vbLf & ex.Message)
                                    Return Nothing
                                End Try

                                '                            
                                '                            The class seems to store the same parameters for other functions. 
                                '                            There is a need to clear the memory
                                '                             
                                fnx.clearParameterMemory()

                                'reset
                                firstOpenBracket = 0
                                lastClosingBracket = calculatorObj.Count - 1
                                searchingIndex = -1

                            Else
                                'return syntax error
                                Console.Error.WriteLine("Error...0x12000 Function Loop Error")
                                Return Nothing
                            End If

                            'if an open bracket is the next object
                        ElseIf TypeOf calculatorObj(searchingIndex + 1) Is CalBracketOpen Then
                            firstOpenBracket = searchingIndex + 1
                            Dim numberOfOpenedBracket As Integer = 0
                            Dim numberOfClosedBracket As Integer = 0

                            Dim i As Integer = firstOpenBracket
                            Do While i <= lastClosingBracket

                                'count the number of brackets
                                If TypeOf calculatorObj(i) Is CalBracketOpen Then
                                    numberOfOpenedBracket += 1
                                ElseIf TypeOf calculatorObj(i) Is CalBracketClose Then
                                    numberOfClosedBracket += 1
                                End If

                                'check if the equal there are equal number of coresponding brackets
                                If numberOfOpenedBracket = numberOfClosedBracket Then
                                    lastClosingBracket = i
                                    Exit Do
                                End If
                                i += 1
                            Loop

                        End If
                    Else
                        'function expression limits where found 
                        If firstOpenBracket <> 0 Then

                            'check if there is a inner function
                            Dim innerFunctionExists As Boolean = False
                            For i As Integer = firstOpenBracket To lastClosingBracket
                                If TypeOf calculatorObj(i) Is GeneralCalFunction Then
                                    innerFunctionExists = True
                                End If
                            Next i
                            If Not innerFunctionExists Then

                                'storage for all local parameter separators
                                Dim parameterIndexList As New List(Of Integer)()

                                'add index of first open bracket
                                parameterIndexList.Add(firstOpenBracket)

                                For i As Integer = firstOpenBracket To lastClosingBracket

                                    'get locations of all local parameter separators
                                    If TypeOf calculatorObj(i) Is CalParameterSeparator Then
                                        parameterIndexList.Add(i)
                                    End If
                                Next i
                                'add index of last closing bracket
                                parameterIndexList.Add(lastClosingBracket)

                                'split the parameter into a list base on the indices
                                Dim parameters(parameterIndexList.Count - 2)() As GeneralCalButton

                                For i As Integer = 0 To parameterIndexList.Count - 1
                                    'JAVA TO VB CONVERTER WARNING: The original Java variable was marked 'final':
                                    'ORIGINAL LINE: final int last_but_one = parameterIndexList.size() - 2;
                                    Dim last_but_one As Integer = parameterIndexList.Count - 2

                                    'calculate number of objects in the between the separators
                                    Dim number_of_objects As Integer = parameterIndexList(i + 1) - parameterIndexList(i) - 1

                                    Dim expression(number_of_objects - 1) As GeneralCalButton

                                    'get expression for parameter
                                    Dim index As Integer = 0
                                    Dim j As Integer = parameterIndexList(i) + 1
                                    Do While j < parameterIndexList(i + 1)
                                        'JAVA TO VB CONVERTER WARNING: An assignment within expression was extracted from the following statement:
                                        'ORIGINAL LINE: expression[index++] = calculatorObj.get(j);
                                        expression(index) = calculatorObj(j)
                                        index += 1
                                        j += 1
                                    Loop

                                    'add expression to list
                                    parameters(i) = expression

                                    If i = last_but_one Then
                                        Exit For
                                    End If
                                Next i

                                'calculate function solution
                                Dim fnx As GeneralCalFunction = CType(calculatorObj(firstOpenBracket - 1), GeneralCalFunction)

                                For Each parameter As GeneralCalButton() In parameters
                                    Dim gv As GeneralCalValue = calculateSolution(parameter)
                                    fnx.addParameter(gv)
                                Next parameter
                                If fnx.parameterCountIsOK() Then
                                    calculatorObj.Insert(firstOpenBracket - 1, fnx.calculate())

                                    'remove calculator objects
                                    For i As Integer = firstOpenBracket - 1 To lastClosingBracket
                                        Dim del As Integer = firstOpenBracket
                                        calculatorObj.RemoveAt(del)
                                    Next i

                                Else
                                    'return syntax error
                                    Console.Error.WriteLine("Error...0x10000 Function Loop Error")
                                    Return Nothing
                                End If

                                'reset
                                firstOpenBracket = 0
                                lastClosingBracket = calculatorObj.Count - 1
                                searchingIndex = -1

                                'clear memory
                                parameterIndexList.Clear()
                                fnx.clearParameterMemory()
                            End If
                        End If
                    End If
                    'update loop
                    searchingIndex += 1
                Loop

                Return calculatorObj
            Catch ex As Exception
                Console.WriteLine("Error...0x5000 Function Loop Error " & ex.Message)
            End Try
            Return Nothing
        End Function

        Private Function bracketAbsLoop(ByVal calculatorObj As List(Of GeneralCalButton)) As List(Of GeneralCalButton)

            Try

                Dim number_of_abs_brackets As Integer = 0
                For i As Integer = 0 To calculatorObj.Count - 1
                    If TypeOf calculatorObj(i) Is CalAbsBracket Then
                        number_of_abs_brackets += 1
                    End If
                Next i

                'process if the abs bracket count is an even number
                If (number_of_abs_brackets Mod 2) = 0 Then

                    If number_of_abs_brackets <> 0 Then
                        'indices for bracket encapsulation
                        Dim last_open_bracket As Integer = -1
                        Dim first_closed_bracket As Integer = -1
                        Dim searching_index As Integer = 0
                        Dim bracket_count As Integer = 0

                        'search for brackets
                        Do While searching_index < calculatorObj.Count
                            If TypeOf calculatorObj(searching_index) Is CalAbsBracket Then
                                bracket_count += 1

                                'when the last open bracket found
                                If (number_of_abs_brackets \ 2) = bracket_count Then
                                    last_open_bracket = searching_index

                                    'find the corresponding closing bracket
                                    For i As Integer = searching_index To calculatorObj.Count - 1
                                        If TypeOf calculatorObj(i) Is CalAbsBracket Then
                                            first_closed_bracket = i
                                        End If
                                    Next i

                                    'if both indices are not -1
                                    If Not (last_open_bracket = -1 OrElse first_closed_bracket = -1) Then

                                        'get local expression and calculate
                                        Dim local_expression As New List(Of GeneralCalButton)()
                                        For i As Integer = last_open_bracket + 1 To first_closed_bracket - 1
                                            local_expression.Add(calculatorObj(i))
                                        Next i

                                        'caculate expression
                                        Dim gv As GeneralCalValue = calculateSolution(functionLoop(local_expression))

                                        'makea a negative answer positive
                                        If gv.my_value < 0 Then
                                            gv.my_value = gv.my_value * -1
                                        End If

                                        'add answer to list
                                        calculatorObj.Insert(last_open_bracket, gv)

                                        'remove bracket expression
                                        For i As Integer = last_open_bracket + 1 To first_closed_bracket
                                            Dim del As Integer = last_open_bracket + 1
                                            calculatorObj.RemoveAt(del)
                                        Next i

                                        'reset 
                                        number_of_abs_brackets -= 2 'remove the two brackets
                                        searching_index = -1
                                        first_closed_bracket = -1
                                        last_open_bracket = -1
                                        local_expression.Clear()
                                    End If
                                End If
                            End If

                            'continue loop 
                            searching_index += 1
                        Loop

                    End If

                Else
                    Console.Error.WriteLine("Error...0x21000 Bracket Absolute Loop Error: Absolute Bracket Count are not even" & number_of_abs_brackets)
                    Return Nothing
                End If

                Return calculatorObj

            Catch ex As Exception
                Console.WriteLine("Error...0x20000 Bracket Absolute Loop Error" & vbLf & ex.Message)
            End Try
            Return Nothing
        End Function

        ''' <summary>
        ''' This method converts the string expression to calculator objects for calculations. </summary>
        ''' <param name="expression"> The expression to calculate. </param>
        ''' <returns> Returns if a boolean indicating if expression was converted successfully  </returns>
        Public Function setExpression(ByVal expression As String) As Boolean

            'remove all white space
            expression = expression.Replace(" ", "")

            clearMemory()

            Dim i As Integer = 0
            Do While i < expression.Length()
                For Each calculatorObj As Enums.EnumCalObjs In Enums.EnumCalObjs.values()

                    Dim index1 As Integer = expression.IndexOf(calculatorObj.getDisplayText1)
                    Dim index2 As Integer = expression.IndexOf(calculatorObj.getDisplayText2)

                    Dim the_object_is_first As Boolean = (index1 = 0 OrElse index2 = 0)

                    If the_object_is_first Then
                        calObjs.Add(calculatorObj.CalculateObject)

                        'get length
                        Dim length_of_finding_text As Integer
                        If index1 = 0 Then
                            length_of_finding_text = calculatorObj.getDisplayText1.Length()
                        Else
                            length_of_finding_text = calculatorObj.getDisplayText2.Length()
                        End If

                        'truncate beginning of the text
                        expression = expression.Substring(length_of_finding_text, expression.Length() - length_of_finding_text)

                        'reset for loop
                        i = -1
                    End If
                Next calculatorObj
                i += 1
            Loop

            'error in converting string
            If expression.Length() > 0 Then
                Console.Error.WriteLine("Error...0x16" & vbLf & "CalQl8R::SetExpression Method Error" & vbLf)
                Dim a As New CalAns(0)
                a.ErrorType = EnumError.SYNTAX
                clearMemory()
                calObjs.Add(a)
                Return False
            End If

            Return True
        End Function

        Public Function getAnswer() As String

            Const NO_LIMITS_TO_SIG_FIGS As Integer = -1
            Return getAnswer(NO_LIMITS_TO_SIG_FIGS)
        End Function

        Public Function getAnswer(number_of_dps As Integer) As String

            If answer.ErrorType Is Enums.EnumError.MATH Then
                Return EnumError.MATH.TEXT
            ElseIf answer.ErrorType Is Enums.EnumError.SYNTAX Then
                Return EnumError.SYNTAX.TEXT
            End If

            'round off ans value to remove unnecessary trailing values
            'get base on answer foramt

            Select Case answer.AnswerFormatType
                Case Enums.EnumAnswerFormat.VALUE
                    'round off ans value to remove unnecessary trailing values
                    Dim ansText As String
                    If number_of_dps = -1 Then
                        ansText = answer.my_value.ToString()
                    Else
                        ansText = answer.getValue(number_of_dps).ToString()
                    End If
                    Return ansText
                Case Enums.EnumAnswerFormat.FRACTION
                    Return answer.GetAnswerFraction
                Case Enums.EnumAnswerFormat.TIME
                    Return answer.GetAnswerTime
                Case Else
            End Select

            Return "0"
        End Function

        ''' <summary>
        ''' This method change the form of the answer in a time form. *Set after the calculator has already run*
        ''' </summary>
        Public Sub setAnswerFormatToTime()

            answer.AnswerFormatType = EnumAnswerFormat.TIME
        End Sub

        ''' <summary>
        ''' This method change the form of the answer in a fraction form. For instance 1.5 to 1/1/2. *Set after the calculator has already run*
        ''' </summary>
        Public Sub setAnswerFormatToFraction()

            answer.AnswerFormatType = EnumAnswerFormat.FRACTION
        End Sub

    End Class

End Namespace