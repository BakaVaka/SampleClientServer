using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace bProxyClient.Shared;

public class DataErrorsHelper<T> {
    private Dictionary<string, List<string>> _errors = [];
    private T? _value;
    private List<Rule> _rules;

    public DataErrorsHelper(T value, params Rule[] rules) {
        _value = value;
        _rules = new List<Rule>(rules);
    }

    public IEnumerable<string> Properties => new HashSet<string>(_rules.Select(x => x.Property));

    public void Validate() {
        _errors = [];
        foreach(var rule in _rules) {
            if(!rule.IsValid(out var error)) {
                _errors.TryAdd(rule.Property, new List<string>());
                _errors[rule.Property].Add(error);
            }
        }
    }

    public Boolean HasErrors => _errors.Any(x => x.Value.Count > 0);

    public IEnumerable GetErrors(String? propertyName) {
        if(_errors.TryGetValue(propertyName, out var errors)){
            foreach(var error in errors) {
                yield return error;
            }
        }
    }

   

    /// <summary>
    /// Правило
    /// </summary>
    public class Rule<TPropertyType>(
        Func<TPropertyType?> accessor,
        string propertyName,
        ValidationDelegate<TPropertyType> validator, 
        string message) : Rule {

        public override string Property { get => propertyName; }
        public override bool IsValid([NotNullWhen(false)]out string? error) {
            error = null;
            var validationResult = true;
            if(!validator(accessor())) {
                error = message;
                validationResult = false;
            }
            return validationResult;
        }
    }
}

public delegate bool ValidationDelegate<TProperty>(TProperty? property);

/// <summary>
/// Правило
/// </summary>
public abstract class Rule {
    public abstract string Property { get; }
    public abstract bool IsValid([NotNullWhen(false)] out string? error);

    public static DataErrorsHelper<T>.Rule<TProperty> For<T, TProperty>(Func<TProperty> accessor, ValidationDelegate<TProperty> validator, string message, string propertyName) {
        return new(accessor, propertyName, validator, message);
    }
}