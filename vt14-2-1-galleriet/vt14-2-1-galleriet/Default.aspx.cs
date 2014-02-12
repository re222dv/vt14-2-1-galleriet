using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using vt14_2_1_galleriet.Model;

namespace vt14_2_1_galleriet {
    public partial class Default : System.Web.UI.Page {
        private IEnumerable<Picture> pictures;

        private IEnumerable<Picture> Pictures {
            get {
                if (pictures == null) {
                    pictures = Gallery.GetPictures().Select(p => {
                        p.Name = HttpUtility.UrlEncode(p.Name);
                        return p;
                    });
                }
                return pictures;
            }
        }

        protected void Page_Load(object sender, EventArgs e) {
            var name = Request.QueryString["name"];
            name = HttpUtility.UrlEncode(name);

            if (Pictures.Any(p => p.Name == name)) {
                Image.ImageUrl = String.Format("Content/Images/{0}", name);
            } else {
                name = Pictures.First().Name;
                Response.Redirect(String.Format("?name={0}", name));
            }

            var success = Request.QueryString["success"];
            if (success != null && success == "success") {
                Success.Visible = true;
            }
        }

        public IEnumerable<Picture> Repeater_GetData() {
            return Pictures;
        }

        protected void Button_Click(object sender, EventArgs e) {
            if (IsValid) {
                try {
                    var name = Gallery.SaveImage(FileUpload.FileContent, FileUpload.FileName);
                    name = HttpUtility.UrlEncode(name);
                    Response.Redirect(String.Format("?name={0}&success=success", name));
                } catch (ArgumentOutOfRangeException) {
                    ModelState.AddModelError(String.Empty, "Endast filtyperna gif, jpeg eller png är tillåtna.");
                } catch (ArgumentException) {
                    ModelState.AddModelError(String.Empty, "Filen verkar inte vara en bild");
                }
            }
        }

        protected void Close_Click(object sender, EventArgs e) {
            var name = Request.QueryString["name"];
            Response.Redirect(String.Format("?name={0}", name));
        }
    }
}