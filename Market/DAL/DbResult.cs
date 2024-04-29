namespace Market.DAL;

public record DbResult(DbResultStatus Status);

public record DbResult<T>(T Result, DbResultStatus Status);