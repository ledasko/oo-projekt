﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StudIS.Desktop.Controllers;
using StudIS.Models;
using StudIS.Models.Users;

namespace StudIS.Desktop
{
    public partial class MainForm : Form
    {
        private readonly MainFormController _mainFormController;
        private readonly LoginFormController _loginFormController;
        private readonly CourseFormController _courseFormController;
        private readonly UserFormController _userFormController;

        public MainForm(
            MainFormController mainFormController, 
            LoginFormController loginFormController,
            CourseFormController courseFormController,
            UserFormController userFormController)
        {
            _mainFormController = mainFormController;
            _loginFormController = loginFormController;
            _courseFormController = courseFormController;
            _userFormController = userFormController;

            InitializeComponent();
            populateCoursesListBox();
        }

        private void populateCoursesListBox()
        {
            this.coursesListBox.DataSource = _mainFormController.GetAllCourses();
            this.coursesListBox.DisplayMember = "Naturalidentifier";
            this.coursesListBox.ValueMember = "Id";
        }

        private bool getSelectedAccountTypeAndEmail(out User user, out string email)
        {
            email = this.emailTextBox.Text;
            user = null;

            if (email.Length == 0)
            {
                MessageBox.Show("Unesite e-mail adresu", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (this.studentRadioButton.Checked)
            {
                user = new Student();
                ((Student)user).CoursesEnrolledIn = new List<Course>();
            }
            else if (this.lecturerRadioButton.Checked)
            {
                user = new Lecturer();
                ((Lecturer)user).CoursesInChargeOf = new List<Course>();
            }
            else if (this.adminRadioButton.Checked)
            {
                user = new Administrator();
            }
            else
            {
                MessageBox.Show("Odaberite vrstu korisnika", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            user.Email = email;
            return true;
        }

        private void newUserButton_Click(object sender, EventArgs e)
        {
            User user;
            string email;

            var valid = getSelectedAccountTypeAndEmail(out user, out email);

            if (valid)
            {
                var success = _userFormController.OpenFormNewUser(user);
                if (!success)
                {
                    MessageBox.Show("Korisnik s unesenom e-mail adresom već postoji", "Korisnik postoji", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void editUserButton_Click(object sender, EventArgs e)
        {
            User user;
            string email;

            var valid = getSelectedAccountTypeAndEmail(out user, out email);

            if (valid)
            {
                var success = _userFormController.OpenFormEditUser(email);
                if (!success)
                {
                    MessageBox.Show("Korisnik s unesenom e-mail adresom ne postoji.", "Ne postoji taj korisnik", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void deleteUserButton_Click(object sender, EventArgs e)
        {
            User user;
            string email;

            var valid = getSelectedAccountTypeAndEmail(out user, out email);

            if (valid)
            {
                var prompt = MessageBox.Show("Zaista izbristi korisnika i sve njegove rezultate?", "Potvrda", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (prompt == DialogResult.No)
                {
                    return;
                }

                var success = _userFormController.DeleteUser(email);
                if (success)
                {
                    MessageBox.Show("Korisnik izbrisan!", "Uspjeh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Korisnik s unesenom e-mail adresom ne postoji", "Nema tog korisnika", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void newCourseButton_Click(object sender, EventArgs e)
        {
            _courseFormController.OpenFormNewCourse();
        }

        private void editCourseButton_Click(object sender, EventArgs e)
        {
            var courseId = (int)this.coursesListBox.SelectedValue;
            _courseFormController.OpenFormEditCourse(courseId);
        }

        private void deleteCourseButton_Click(object sender, EventArgs e)
        {
            var courseId = (int)this.coursesListBox.SelectedValue;

            var prompt = MessageBox.Show("Izbrisati predmet i sve rezultate?", "Potvrda brisanja", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (prompt == DialogResult.No)
            {
                return;
            }
            
            bool success = _courseFormController.DeleteCourse(courseId);
            if(!success) {
                MessageBox.Show("Došlo je do greške prilikom brisanja", "Neuspjeh", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            populateCoursesListBox();
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            populateCoursesListBox();
        }
    }
}
