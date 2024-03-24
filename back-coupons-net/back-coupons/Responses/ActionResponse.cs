namespace back_coupons.Responses
{
    public class ActionResponse<T>
    {
        public bool Successfully { get; set; }
        public string ?Message {  get; set; }
        public T? Result { get; set; }
    }
}
