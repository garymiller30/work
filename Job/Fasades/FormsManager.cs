namespace Job.Fasades
{
//    public class FormsManager
//    {
//
//        public Profile UserProfile { get; set; }
//
//        const string CollectionStringForms = "forms";
//        const string CollectionStringKnownForms = "KnownForms";
//
//        public string WorkPath;
//
//        private List<Forms> _formses = new List<Forms>();
//
//        List<Forms> _knownForms = new List<Forms>();
//
//
//        public delegate void EventHandler(Forms form);
//
//        public event EventHandler OnPlateChange = delegate{};
//        public event EventHandler OnPlateAdd = delegate { };
//        public event EventHandler OnPlateRemove = delegate { };
//        public event EventHandler<IEnumerable<Forms>> OnPlatesAdd = delegate { };
//
//
//        public FormsManager()
//        {
//            //MongoAdd();
//        }
//
//        public void Connect(bool reconnect)
//        {
//
//            //if (string.IsNullOrEmpty(MongoDbServer)) return;
//            try
//            {
//                if (!reconnect)
//                {
//                    UserProfile.MQ.OnPlateChanged += MQ_OnPlateChanged;
//                    UserProfile.MQ.OnPlateAdd += MQ_OnPlateAdd;
//                    UserProfile.MQ.OnPlateRemove += MQ_OnPlateRemove;
//                }
//
//
//                //LoadKnownForms();
//            }
//            catch (Exception e)
//            {
//                MessageBox.Show(e.Message);
//
//            }
//        }
//
//        void MQ_OnPlateRemove(object sender,ObjectId id)
//        {
//
//            var o = (Forms) UserProfile.Base.GetById<Forms>(CollectionStringForms, id);
//            if (o != null)
//            {
//                var ff = _formses.FirstOrDefault(x => x.Id.Equals(o.Id));
//                if (ff != null)
//                {
//                    _formses.Remove(ff);
//
//                    OnPlateRemove(ff);
//                }
//            }
//        }
//
//        void MQ_OnPlateAdd(object sender,ObjectId id)
//        {
//
//             var o = (Forms) UserProfile.Base.GetById<Forms>(CollectionStringForms, id);
//            if (o != null)
//            {
//                _formses.Add(o);
//                OnPlateAdd(o);
//            }
//        }
//
//        void MQ_OnPlateChanged(object sender, ObjectId id)
//        {
//              var o = (Forms) UserProfile.Base.GetById<Forms>(CollectionStringForms, id);
//            if (o != null)
//            {
//                var ff = _formses.FirstOrDefault(x => x.Id.Equals(o.Id));
//                if (ff != null)
//                {
//                    ff.Update(o);
//                    OnPlateChange(ff);
//                }
//
//            }
//        }
//
//
//         public void Load()
//         {
//
//            var cltn = UserProfile.Base.GetCollection<Forms>(CollectionStringForms);
//            if (cltn != null) _formses = cltn;
//
//            cltn = UserProfile.Base.GetCollection<Forms>(CollectionStringKnownForms);
//            if (cltn != null) _knownForms = cltn;
//             
//            ResetSelected();
//            OnPlatesAdd(this, _formses);
//         }
//
///*
//        public Forms AddKnownForm(int width,int height)
//        {
//            var f = new Forms(width,height);
//
//            if (Managers.Base.Add(CollectionStringKnownForms, f))
//            {
//                _knownForms.Add(f);
//            }
//            return f;
//        }
//*/
//
//
//        private bool IsPresentKnownForms(Forms form)
//        {
//            return !_knownForms.Contains(form);
//        }
//
//        public void RemoveKnownForm(Forms form)
//        {
//
//            if (UserProfile.Base.Remove(CollectionStringKnownForms, form))
//            {
//                _knownForms.Remove(form);
//            }
//
//        }
//
//        public Forms[] GetKnowsFormses()
//        {
//            return _knownForms.ToArray();
//        }
//
//        public Forms AddKnownForm(string str)
//        {
//            int w, h;
//            bool r = SeparateFormatString(str, out w, out h);
//
//            if (r)
//            {
//                var form = new Forms(w,h);
//                if (IsPresentKnownForms(form))
//                {
//                    if (UserProfile.Base.Add(CollectionStringKnownForms, form))
//                    {
//                        _knownForms.Add(form);
//                        return form;
//                    }
//                }
//            }
//            return null;
//        }
//
//
//        private bool SeparateFormatString(string str, out int w, out int h)
//        {
//            w= h = 0;
//
//            Regex regex = new Regex(@"\d+");
//            Match match = regex.Match(str);
//            if (match.Success)
//            {
//                w = int.Parse(match.Value);
//
//                regex = new Regex(@"\d+$");
//                match = regex.Match(str);
//                if (match.Success)
//                {
//                    h = int.Parse(match.Value);
//                    return true;
//                }
//
//            }
//            return false;
//
//        }
//
//        public void Add(Forms f)
//        {
//            if (UserProfile.Base.Add(CollectionStringForms, f))
//            {
//                _formses.Add(f);
//                UserProfile.MQ.PublishChanges(MessageEnum.PlateAdd, f.Id);
//            }
//            
//        }
//
//        public void Remove(Forms f)
//        {
//            if (UserProfile.Base.Remove(CollectionStringForms, f))
//            {
//                _formses.Remove(f);
//                UserProfile.MQ.PublishChanges(MessageEnum.PlateRemove, f.Id);
//            }
//        }
//
//
//        public  void Apply(Job j)
//        {
//            if (j.Parts != null)
//            {
//                foreach (Part part in j.Parts)
//                {
//                    var c = part.GetPlottedDelta(part.Form.Format);
//                    if (c > 0)
//                    {
//                        var form = CustomerPresent(part.Form);
//
//                        if (form != null)
//                        {
//                            form.Count -= c;
//                            JobLogger.WriteLine(
//                                $"{j.Number}\t{j.Customer}: {form.Format} = -{c}({form.Count}) (Owner: {UserProfile.PlateOwners.GetOwnerNameById( form.Format.OwnerId)})");
//                            
//                            MongoRefresh(form);
//                        }
//                    }
//                }
//            }
//        }
//
//        private Forms CustomerPresent(Forms forms)
//        {
//            return _formses.FirstOrDefault(f => f.Format.Equals(forms.Format) && f.Format.OwnerId.Equals(forms.Format.OwnerId));
//        }
//
//        private void MongoRefresh(Forms form)
//        {
//            try
//            {
//                UserProfile.Base.Update(CollectionStringForms, form);
//                UserProfile.MQ.PublishChanges(MessageEnum.PlateChange, form.Id);
//            }
//            catch (Exception e)
//            {
//               
//            }
//           
//        }
//
///*
//        private  Forms CustomerPresent(string cust)
//        {
//            return _formses.FirstOrDefault(formse => formse.Owner != null && formse.Owner.Equals(cust));
//        }
//*/
//
///*
//        private  Forms GetCommonsForms(PlateFormat pf)
//        {
//            return _formses.FirstOrDefault(f => f.Format.Equals(pf) && String.IsNullOrEmpty(f.Owner));
//        }
//*/
//
//        public  string GetPlannedFormForJob(Job j)
//        {
//            var sb = new StringBuilder();
//
//            if (j.Parts != null)
//            {
//                var g = j.Parts.GroupBy(x => x.Form);
//
//                foreach (var part in g)
//                {
//                    var c = part.Sum(x => x.GetCountUnplottedForms(x.Form));
//                    var d = part.Sum((x => x.Form.Count));
//                        sb.AppendFormat("[{0}: {2}\\{1}]", part.Key.Format, d-c,d);
//                }
//            }
//
//            return sb.ToString();
//        }
//
//        public  void ResetSelected()
//        {
//            _formses.Any(x => x.IsSelected = false);
//
//        }
//
//        public  void MarkSelected(Forms form)
//        {
//            var q = _formses.Where(o => o.Format.Equals(form.Format) && o.Format.OwnerId.Equals(form.Format.OwnerId));
//
//            q.Any(x => x.IsSelected = true);
//
//        }
//
//        public  IList GetListForms()
//        {
//            return _formses;
//        }
//
///*
//        public bool IsCustomerHaveForms(PlateFormat pf, string customer)
//        {
//            var q = _formses.Where(o => o.Owner.Equals(customer) && o.Format.Equals(pf));
//
//            return q.Any();
//
//        }
//*/
//
//        public void Refresh(Forms forms)
//        {
//            MongoRefresh(forms);
//        }
//
///*
//        public List<string> GetCustomersName()
//        {
//            var g = _formses.GroupBy(x => x.Owner);
//            return g.Select(grouping => grouping.Key).ToList();
//        }
//*/
//
//
//        public string GetBrakFormForJob(Job j)
//        {
//            var sb = new StringBuilder();
//
//            if (j.Parts != null)
//            {
//                var g = j.Parts.GroupBy(x => x.Form);
//
//                foreach (var part in g)
//                {
//                    var d = part.Sum((x => x.Form.Brak));
//                    if (d>0)
//                    sb.AppendFormat("[{0}: {1}]", part.Key.Format, d);
//                }
//            }
//
//            return sb.ToString();
//        }
//    }
}
