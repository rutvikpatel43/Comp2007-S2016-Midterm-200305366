﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// using statements required for EF DB access
using COMP2007_S2016_MidTerm_200305366.Models;
using System.Web.ModelBinding;

namespace COMP2007_S2016_MidTerm_200305366
{
    public partial class TodoDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!IsPostBack) && (Request.QueryString.Count > 0))
            {
                this.GetTodo();
            }
        }
        protected void GetTodo()
        {
            // populate teh form with existing data from the database
            int TodoID = Convert.ToInt32(Request.QueryString["TodoID"]);

            // connect to the EF DB
            using (TodoConnection db = new TodoConnection())
            {
                // populate a student object instance with the StudentID from the URL Parameter
                Todo updatedTodo = (from todo in db.Todos
                                          where todo.TodoID == TodoID
                                          select todo).FirstOrDefault();

                // map the student properties to the form controls
                if (updatedTodo != null)
                {
                    TodoNameTextBox.Text = updatedTodo.TodoName;
                    TodoNotesTextBox.Text = updatedTodo.TodoNotes;
                    Completed.Checked = Convert.ToBoolean(updatedTodo.Completed);
                }
            }
        }
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("../TodoList.aspx");
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            // Use EF to connect to the server
            using (TodoConnection db = new TodoConnection())
            {
                // use the Student model to create a new student object and
                // save a new record
                Todo newtodo = new Todo();

                int TodoID = 0;

                if (Request.QueryString.Count > 0) // our URL has a StudentID in it
                {
                    // get the id from the URL
                    TodoID = Convert.ToInt32(Request.QueryString["TodoID"]);

                    // get the current student from EF DB
                    newtodo = (from todo in db.Todos
                                  where todo.TodoID == TodoID
                                  select todo).FirstOrDefault();
                }

                // add form data to the new student record
                newtodo.TodoName = TodoNameTextBox.Text;
                newtodo.TodoNotes = TodoNotesTextBox.Text;
                newtodo.Completed = Completed.Checked;

                // use LINQ to ADO.NET to add / insert new student into the database

                if (TodoID == 0)
                {
                    db.Todos.Add(newtodo);
                }


                // save our changes - also updates and inserts
                db.SaveChanges();

                // Redirect back to the updated students page
                Response.Redirect("./TodoList.aspx");
            }
        }
    }
}