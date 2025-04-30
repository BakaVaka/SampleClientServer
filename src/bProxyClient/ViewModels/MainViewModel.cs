using System.ComponentModel;
using System;

using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections;
using bProxyClient.Services;
using bProxyClient.Shared;
using System.Net;

namespace bProxyClient.ViewModels;

public partial class MainViewModel : ViewModelBase, INotifyDataErrorInfo
{
    private readonly DataErrorsHelper<MainViewModel> _validationHelper;
    
    [ObservableProperty]
    private string _connectionStatus = "Disconnected";

    [ObservableProperty]
    private string _ipAddress;

    [ObservableProperty]
    private string _port;

    public MainViewModel() {
        _validationHelper = new DataErrorsHelper<MainViewModel>
            (
                this,
                Rule.For<ProxyClientService, string> 
                (
                    () => IpAddress,
                    (str) => IPAddress.TryParse(str, out _),
                    "Invalid IP address format",
                    nameof(IpAddress)
                ),

                Rule.For<ProxyClientService, string>
                (
                    () => Port,
                    (str) => UInt16.TryParse(str, out _),
                    "Invalid port value (Valid value: 1..65535)",
                    nameof(Port)
                )
            );

            }

    partial void OnIpAddressChanged(String value) => Validate();
    partial void OnPortChanged(string value) => Validate();
    private void Validate() {
        _validationHelper.Validate();
        foreach(var prop in _validationHelper.Properties) {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(prop));
        }

    }

    public Boolean HasErrors => _validationHelper.HasErrors;
    public IEnumerable GetErrors(String? propertyName) => _validationHelper.GetErrors(propertyName);

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
}
