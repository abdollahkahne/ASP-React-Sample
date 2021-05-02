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
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        AnswerGetResponse IDataRepository.GetAnswer(int answerId)
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

        QuestionGetSingleResponse IDataRepository.GetQuestion(int questionId)
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
                    question.Answers=connection.Query<AnswerGetResponse>(getAnswerSql,new {questionId=questionId});
                }
                return question;
            }
        }

        IEnumerable<QuestionGetManyResponses> IDataRepository.GetQuestionBySearch(string search)
        {
            string sql=@"SELECT QuestionId, Title, Content, UserId, UserName, Created
             FROM Question WHERE Title LIKE '%' + @Search + '%'	
             UNION 
             SELECT QuestionId, Title, Content, UserId, UserName, Created
		    FROM Question 
		    WHERE Content LIKE '%' + @Search + '%'";
            using (var connection= new SqliteConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<QuestionGetManyResponses>(sql,new {search=search});
            }
        }

        IEnumerable<QuestionGetManyResponses> IDataRepository.GetQuestions()
        {
            string sql="SELECT QuestionId, Title, Content, UserId, UserName, Created FROM Question";
            using (var connection= new SqliteConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<QuestionGetManyResponses>(sql);
            }
        }

        IEnumerable<QuestionGetManyResponses> IDataRepository.GetUnAnsweredQuestions()
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

        bool IDataRepository.QuestionExists(int questionId)
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