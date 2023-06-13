using RetailStore.Model;

namespace RetailStore.Dtos;

/// <summary>
/// Data transfer object for querying Customer. 
/// <see cref="EmployeeDto"/>
/// </summary>
public class CustomerDto
{

    /// <summary>
    /// Unique identification number for each record of customer entity
    /// </summary>
    /// <example>
    /// 1
    /// </example>
    public int Id { get; set; }


    /// <summary>
    /// Represents name of customer entity 
    /// </summary>
    /// <example>
    /// "Jhon Smith"
    /// </example>
    public string Name { get; set; }

    /// <summary>
    /// Represents phone number of customer entity
    /// </summary>
    /// <example>
    /// 1122334455
    /// </example>
    public int PhoneNumber { get; set; }

}
