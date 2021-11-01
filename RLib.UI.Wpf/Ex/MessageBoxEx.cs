/********************************************************************
    created:	2019/1/7 15:14:26
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RLib.Base;

namespace RLib.Base
{
    public static class MessageBoxEx
    {
        public static async Task<MessageBoxResult> ShowAsync(string messageBoxText, string caption ="注意", MessageBoxButton button= MessageBoxButton.OK,  MessageBoxImage icon= MessageBoxImage.None, MessageBoxResult defaultResult = MessageBoxResult.None, MessageBoxOptions options = 0)
        {
                return await Application.Current.Dispatcher.InvokeAsync<MessageBoxResult>(
                        () => { return MessageBox.Show(messageBoxText, caption, button, icon, defaultResult, options); }
                   );
        }

        //public static async Task<MessageBoxResult> ShowAsync(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult )
        //{
        //    return await ShowAsync(messageBoxText, caption, button, icon, defaultResult);

        //}

        //public static async Task<MessageBoxResult> ShowAsync(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
        //{

        //}
    
        //public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button);
     
        //public static MessageBoxResult Show(string messageBoxText, string caption);
    
        //public static MessageBoxResult Show(string messageBoxText);

    }
}
