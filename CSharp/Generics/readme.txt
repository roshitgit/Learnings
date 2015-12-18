
** convert a type T to a specific type
http://stackoverflow.com/questions/4092393/value-of-type-t-cannot-be-converted-to


ex:
try{


}
catch (UriFormatException ex)
            {
                SendMailOnException(ex);
            }
            catch (WebException ex)
            {
                SendMailOnException(ex);
            }
            catch (Exception ex)
            {
                SendMailOnException(ex);
            }


private static void SendMailOnException<T>(T ex)
        {
            dynamic result = (T)(object)ex;

            new Emailer().Publish(new CustomException { ErrorMessage = result.Message, 
                MethodName = new StackTrace().GetFrame(1).GetMethod().Name, 
                StackTrace = result.StackTrace, Source = result.Source });

            throw new HttpResponseException
               (new CodeHelpers().GetHttpResponseException(result.Message));
        }            

