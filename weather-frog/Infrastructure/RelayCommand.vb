
Namespace Infrastructure

    Public Class RelayCommand(Of T)
        Implements ICommand

#Region "Declarations"

        Private ReadOnly _canExecute As Predicate(Of T) = Nothing
        Private ReadOnly _execute As Action(Of T) = Nothing

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RelayCommand(Of T)"/> class, the command can always be executed.
        ''' </summary>
        ''' <param name="execute">The execution logic.</param>
        Public Sub New(ByVal execute As Action(Of T))
            Me.New(execute, Nothing)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RelayCommand(Of T)"/> class.
        ''' </summary>
        ''' <param name="execute">The execution logic.</param>
        ''' <param name="canExecute">The execution status logic.</param>
        Public Sub New(ByVal execute As Action(Of T), ByVal canExecute As Predicate(Of T))

            If execute Is Nothing Then
                Throw New ArgumentNullException("execute")
            End If
            _execute = execute
            _canExecute = canExecute
        End Sub

#End Region

#Region "ICommand Members"

        Public Custom Event CanExecuteChanged As EventHandler Implements System.Windows.Input.ICommand.CanExecuteChanged
            AddHandler(ByVal value As EventHandler)

                If _canExecute IsNot Nothing Then
                    AddHandler CommandManager.RequerySuggested, value
                End If

            End AddHandler
            '
            RemoveHandler(ByVal value As EventHandler)

                If _canExecute IsNot Nothing Then
                    RemoveHandler CommandManager.RequerySuggested, value
                End If

            End RemoveHandler
            '
            RaiseEvent(ByVal sender As Object, ByVal e As System.EventArgs)

                If _canExecute IsNot Nothing Then
                    CommandManager.InvalidateRequerySuggested()
                End If

            End RaiseEvent
        End Event

        <DebuggerStepThrough>
        Public Function CanExecute(ByVal parameter As Object) As Boolean Implements ICommand.CanExecute
            Return If(_canExecute Is Nothing, True, _canExecute(CType(parameter, T)))
        End Function

        Public Sub Execute(ByVal parameter As Object) Implements ICommand.Execute
            _execute(CType(parameter, T))
        End Sub

#End Region
    End Class

    ''' <summary>
    ''' A command whose sole purpose is to relay its functionality to other objects by invoking delegates. The default return value for the CanExecute method is 'true'.
    ''' </summary>
    Public Class RelayCommand
        Implements ICommand

#Region "Declarations"

        Private ReadOnly _canExecute As Func(Of Boolean)
        Private ReadOnly _execute As Action

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RelayCommand"/> class and the command can always be executed.
        ''' </summary>
        ''' <param name="execute">The execution logic.</param>
        Public Sub New(ByVal execute As Action)
            Me.New(execute, Nothing)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RelayCommand"/> class.
        ''' </summary>
        ''' <param name="execute">The execution logic.</param>
        ''' <param name="canExecute">The execution status logic.</param>
        Public Sub New(ByVal execute As Action, ByVal canExecute As Func(Of Boolean))

            If execute Is Nothing Then
                Throw New ArgumentNullException("execute")
            End If
            _execute = execute
            _canExecute = canExecute
        End Sub

#End Region

#Region "ICommand Members"

        Public Custom Event CanExecuteChanged As EventHandler Implements System.Windows.Input.ICommand.CanExecuteChanged
            AddHandler(ByVal value As EventHandler)

                If _canExecute IsNot Nothing Then
                    AddHandler CommandManager.RequerySuggested, value
                End If

            End AddHandler
            '
            RemoveHandler(ByVal value As EventHandler)

                If _canExecute IsNot Nothing Then
                    RemoveHandler CommandManager.RequerySuggested, value
                End If

            End RemoveHandler
            '
            RaiseEvent(ByVal sender As Object, ByVal e As System.EventArgs)

                If _canExecute IsNot Nothing Then
                    CommandManager.InvalidateRequerySuggested()
                End If

            End RaiseEvent
        End Event

        <DebuggerStepThrough>
        Public Function CanExecute(ByVal parameter As Object) As Boolean Implements ICommand.CanExecute
            Return If(_canExecute Is Nothing, True, _canExecute())
        End Function

        Public Sub Execute(ByVal parameter As Object) Implements ICommand.Execute
            _execute()
        End Sub

#End Region

    End Class

End Namespace
