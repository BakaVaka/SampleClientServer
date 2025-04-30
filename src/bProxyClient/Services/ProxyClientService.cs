using System;
using System.Collections;
using System.ComponentModel;
using System.Net;

using bProxyClient.Shared;

using CommunityToolkit.Mvvm.ComponentModel;

namespace bProxyClient.Services;
public partial class ProxyClientService : ObservableObject, INotifyDataErrorInfo {

    [ObservableProperty]
    private string _ipAddress;

    [ObservableProperty]
    private string _port;

    private DataErrorsHelper<ProxyClientService> _validationHelper;

    public ProxyClientService() {
        _validationHelper = new DataErrorsHelper<ProxyClientService>(
            this,
            Rule.For<ProxyClientService, string>(
                () => IpAddress,
                (str) => IPAddress.TryParse(str, out _),
                "Invalid IP address",
                nameof(IpAddress))
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
