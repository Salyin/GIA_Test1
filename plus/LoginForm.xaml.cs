using System.Diagnostics;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GroceryApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
		DbConnection.Initialize();

	}

	public async void AuthApprBtn_Click(object sender, RoutedEventArgs e)
	{
		string userName = UsernameField.Text;
		string userPass = SecureStringToString(PassField.SecurePassword);

		if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(userPass))
		{
			if (userName.Length > 256 || userPass.Length < 5 || userPass.Length > 256)
			{
                await FlashErrorMessage("Ошибка: неверная длина логина или пароля.");
			}
			else
			{
				if (!Regex.IsMatch(userName, @"^[a-zA-Z0-9_#]+$"))
				{
                    await FlashErrorMessage("Ошибка: логин содержит недопустимые символы.");
				}
				else
				{
					LoginBody(userName, userPass);
				}
			}
		}
		else
		{
            await FlashErrorMessage("Ошибка: Поле логина и пароя не должно быть пустым.");
		}

	}

	public async void RAuthApprBtn_Click(object sender, RoutedEventArgs e)
	{
		string userName = RUsernameField.Text;
		string userPass = SecureStringToString(RPassField.SecurePassword);

		if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(userPass))
		{
			if (userName.Length > 256 || userPass.Length < 5 || userPass.Length > 256)
			{
                await RFlashErrorMessage("Ошибка: неверная длина логина или пароля.");
			}
			else
			{
				if (!Regex.IsMatch(userName, @"^[a-zA-Z0-9_#]+$"))
				{
                    await RFlashErrorMessage("Ошибка: логин содержит недопустимые символы.");
				}
				else
				{
					RegistrationBody(userName, userPass);
				}
			}                
		}
		else
		{
            await RFlashErrorMessage("Ошибка: Поле логина и пароя не должно быть пустым.");
		}
	}


	public void ToRegistration_Click(object sender, RoutedEventArgs e)
	{
		ToRegistrationBody();
	}

	public void ToRegistrationBody()
	{
		loginMenu.Visibility = Visibility.Hidden;
		UsernameField.Clear();
		PassField.Clear();
        ErrorMessageLabel.Content = "";

        RegMenu.Visibility = Visibility.Visible;
	}

	public void ToLogin_Click(object sender, RoutedEventArgs e)
	{
		ToLoginBody();
	}

	public void ToLoginBody()
	{
		RegMenu.Visibility = Visibility.Hidden;
		RUsernameField.Clear();
		RPassField.Clear();
        ErrorMessageLabel.Content = "";

        loginMenu.Visibility = Visibility.Visible;
	}

    public async void LoginBody(string userName, string userPass)
    {
        int v_userId = DbConnection.CheckAccount(userName, userPass);
        if (v_userId != -1)
        {
            ErrorMessageLabel.Content = "Аккаунт существует, вход в Главное меню...";

            var v_mainMenu = new MainMenu(v_userId);
            v_mainMenu.Show();

            Close();
        }
        else
        {
            await FlashErrorMessage("Ошибка: Аккаунт не существует, либо пароль неверен.");
            var storyboard = (Storyboard)FindResource("FlashAnimation");
            storyboard.Begin(ErrorMessageLabel);

            await Task.Delay(3000);
        }
    }

    public async void RegistrationBody(string userName, string userPass)
	{
		var v_userId = DbConnection.CreateAccount(userName, userPass);
		if (v_userId != -1)
		{
			RErrorMessageLabel.Content = "Аккаунт успешно создан! Войдите в свой новый аккаунт в окне авторизации...";
			await Task.Delay(1000);
			ToLoginBody();
		}
		else
		{

            await RFlashErrorMessage("Ошибка: Что-то пошло не так во время создания аккаунта.");
            var storyboard = (Storyboard)FindResource("FlashAnimation");
            storyboard.Begin(RErrorMessageLabel);
            await Task.Delay(3000);
        }
	}

    private async Task FlashErrorMessage(string message)
    {
        
        var originalColor = ErrorMessageLabel.Foreground;

        
        ErrorMessageLabel.Foreground = Brushes.Red;
        ErrorMessageLabel.Content = message;

        
        for (int i = 0; i < 3; i++)
        {
            ErrorMessageLabel.Opacity = 0.5;
            await Task.Delay(100);
            ErrorMessageLabel.Opacity = 1;
            await Task.Delay(100);
        }

        
        await Task.Delay(100);
        ErrorMessageLabel.Foreground = originalColor;
    }

    private async Task RFlashErrorMessage(string message)
    {

        var originalColor = RErrorMessageLabel.Foreground;


        RErrorMessageLabel.Foreground = Brushes.Red;
        RErrorMessageLabel.Content = message;


        for (int i = 0; i < 3; i++)
        {
            RErrorMessageLabel.Opacity = 0.5;
            await Task.Delay(100);
            RErrorMessageLabel.Opacity = 1;
            await Task.Delay(100);
        }


        await Task.Delay(100);
        RErrorMessageLabel.Foreground = originalColor;
    }
    public string SecureStringToString(SecureString secureString)
	{
		IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(secureString);

		try
		{
			return System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
		}
		finally
		{
			System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
		}
	}

	/* private void LoginBtn_Click(object sender, RoutedEventArgs e)
	{
		int v_userId = DbConnection.CreateOrGetUser(LoginTb.Text, PasswordTb.Text);
		if (v_userId == -1)
		{
			MessageBox.Show(this, "Не удалось совершить авторизацию", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			return;
		}

		var v_mainMenu = new MainMenu(v_userId);
		v_mainMenu.Show();

		Close();
	}
	*/
}