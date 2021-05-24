namespace MvcPractice.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Data.Entity.Validation;

    public class MvcPracticeContext : DbContext
    {
        // 您的內容已設定為使用應用程式組態檔 (App.config 或 Web.config)
        // 中的 'MvcPractice' 連接字串。根據預設，這個連接字串的目標是
        // 您的 LocalDb 執行個體上的 'MvcPractice.Models.MvcPractice' 資料庫。
        // 
        // 如果您的目標是其他資料庫和 (或) 提供者，請修改
        // 應用程式組態檔中的 'MvcPractice' 連接字串。
        public MvcPracticeContext()
            : base("name=MvcPractice")
        {
        }

        // 針對您要包含在模型中的每種實體類型新增 DbSet。如需有關設定和使用
        // Code First 模型的詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=390109。

         public virtual DbSet<Member> Members { get; set; }

       
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException exception)
            {
                var sb = new StringBuilder();
                foreach (var failure in exception.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }
                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), exception);
            }
        }
    }
}