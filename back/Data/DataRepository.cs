using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Dapper;
using Microsoft.Data.Sqlite;

namespace back.Data
{
    public class DataRepository : IDataRepository
    {
        private readonly string _connectionString;

        public DataRepository(IConfiguration configuration)
        {
            // _connectionString=configuration.GetConnectionString("DefaultConnection");

            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public void DeleteQuestion(int questionId)
        {
            var sql=@"
            DELETE
	        FROM Question
	        WHERE QuestionID = @QuestionId
            ";
            using (var connection=new SqliteConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(sql, new {QuestionId=questionId});
            }
        }

        public AnswerGetResponse PostAnswer(AnswerPostRequest answer)
        {
            string sql=@"
            INSERT INTO Answer
		        (QuestionId, Content, UserId, UserName, Created)
            Values(@QuestionId, @Content, @UserId, @UserName, @Created);

            SELECT AnswerId, Content, UserName, UserId, Created
            FROM Answer
            WHERE AnswerId = last_insert_rowid()
            ";

            using (var connection=new SqliteConnection(_connectionString))
            {
                connection.Open();
                return connection.QueryFirst<AnswerGetResponse>(sql, answer);
            }
        }

        public QuestionGetSingleResponse PostQuestion(QuestionPostRequest question)
        {
            string sql=@"
            INSERT INTO Question
		    (Title, Content, UserId, UserName, Created)
	        VALUES(@Title, @Content, @UserId, @UserName, @Created);
	        SELECT last_insert_rowid() AS QuestionId
            ";
            using (var connection=new SqliteConnection(_connectionString))
            {
                connection.Open();
                int questionId=connection.QueryFirst<int>(sql,question);
                return GetQuestion(questionId);

            }
        }

        public QuestionGetSingleResponse PutQuestion(int questionId, QuestionPutRequest question)
        {
            string sql=@"
            UPDATE Question
            SET Title = @Title, Content = @Content
            WHERE QuestionID = @QuestionId
            ";
            using (var connection=new SqliteConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(sql,new {QuestionId = questionId,question.Title,question.Content});
                return GetQuestion(questionId);
            }
        }

        public AnswerGetResponse GetAnswer(int answerId)
        {
            string sql=@"
            SELECT AnswerId, Content, Username, Created
            FROM Answer 
            WHERE AnswerId = @AnswerId
            ";
            using (var connection= new SqliteConnection(_connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<AnswerGetResponse>(sql,new {AnswerId=answerId});
            }
        }
        public QuestionGetSingleResponse GetQuestion(int questionId)
        {
            string sql=@"
            SELECT QuestionId, Title, Content, UserId, Username, Created
	        FROM Question 
	        WHERE QuestionId = @QuestionId
            ";
            string getAnswerSql=@"            
	        SELECT AnswerId, QuestionId, Content, Username, Created
	        FROM Answer 
	        WHERE QuestionId = @QuestionId
            ";
            using (var connection=new SqliteConnection(_connectionString))
            {
                connection.Open();
                var question=connection.QueryFirstOrDefault<QuestionGetSingleResponse>(sql,new {QuestionId=questionId});
                if (question!=null) {
                    question.Answers=connection.Query<AnswerGetResponse>(getAnswerSql,new {QuestionId=questionId});
                }
                return question;
            }
        }

        public IEnumerable<QuestionGetManyResponses> GetQuestionBySearch(string search)
        {
            string sql=@"SELECT QuestionId, Title, Content, UserId, UserName, Created
             FROM Question WHERE Title LIKE @Search	
             UNION 
             SELECT QuestionId, Title, Content, UserId, UserName, Created
		    FROM Question 
		    WHERE Content LIKE @Search";
            
            using (var connection= new SqliteConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<QuestionGetManyResponses>(sql,new {Search='%'+search+'%'});
            }
        }


        public IEnumerable<QuestionGetManyResponses> GetQuestions()
        {
            string sql="SELECT QuestionId, Title, Content, UserId, UserName, Created FROM Question";
            using (var connection= new SqliteConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<QuestionGetManyResponses>(sql);
            }
        }

        public IEnumerable<QuestionGetManyResponses> GetUnAnsweredQuestions()
        {
            string sql=@"SELECT QuestionId, Title, Content, UserId, UserName, Created
	        FROM Question q
	        WHERE NOT EXISTS (SELECT *
	        FROM Answer a
	        WHERE a.QuestionId = q.QuestionId)";
            using (var connection= new SqliteConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<QuestionGetManyResponses>(sql);
            }
        }


        public bool QuestionExists(int questionId)
        {
            string sql=@"            
            SELECT CASE WHEN EXISTS (SELECT QuestionId
            FROM Question
            WHERE QuestionId = @QuestionId) 
            THEN CAST (1 AS BIT) 
            ELSE CAST (0 AS BIT) END AS Result
            ";
            using (var connection= new SqliteConnection(_connectionString))
            {
                connection.Open();
                return connection.QueryFirst<bool>(sql,new {QuestionId=questionId});
            }
        }
    }
}