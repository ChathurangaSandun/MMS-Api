using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using MMS.Api.Common.Paginations;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMS.Api.Common.Filters
{
    public class PaginationOptionFilter : IActionFilter
    {
        private readonly PaginationOption defaultPaginationOption;
        public PaginationOptionFilter(IOptions<PaginationOption> option)
        {
            this.defaultPaginationOption = option.Value;
        }

        public void OnActionExecuted(ActionExecutedContext context) { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // get query params
            var argumentValues = context.ActionArguments.Values;
            int page = 0;
            int size = 0;

            var isIncludePaginationOption = false;
            // looping the argument values
            foreach (var value in argumentValues)
            {
                // check value type quals to PaginationOption class
                if (value.GetType().Equals(typeof(PaginationOption)))
                {
                    // if it is true cast the value object
                    page = ((PaginationOption)value).Page;
                    size = ((PaginationOption)value).Size;

                    // if page and size are equal and less than zero set the defult values from appsettings
                    if (page <= 0)
                    {
                        page = this.defaultPaginationOption.Page;
                    }

                    if (size <= 0)
                    {
                        size = this.defaultPaginationOption.Size;
                    }

                    // set the values again to context argumetns
                    ((PaginationOption)value).Page = page;
                    ((PaginationOption)value).Size = size;

                    isIncludePaginationOption = true;
                    break;
                }
            }

            // return error when filter pitting without adding qury parameter
            if (!isIncludePaginationOption)
            {
                throw new Exception("Please add [FromQuery] PaginationOption object");
            }
        }
    }
}
